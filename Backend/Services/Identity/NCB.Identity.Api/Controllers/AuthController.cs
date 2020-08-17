using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NCB.Core.Filters;
using NCB.Core.Helpers;
using NCB.Core.Models;
using NCB.Core.Utilities;
using NCB.EventBus.Abstractions;
using NCB.Identity.Api.Application.DTOs;
using NCB.Identity.Api.DataAccess.BaseRepository;
using NCB.Identity.Api.DataAccess.BaseUnitOfWork;
using NCB.Identity.Api.DataAccess.Entities;
using NCB.Identity.Api.Infrastructure.Exceptions;
using NCB.Identity.Api.IntegraionEvents.Events;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NCB.Identity.Api.Controllers
{
    [Route("api/identity/auth")]
    //[CustomAuthorize(Roles = new string[] { "Admin" })]
    //[CustomAuthorizeFilter(roles: new string[] { "Admin" })]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBaseRepository<Account> _accountRepository;
        private readonly IBaseRepository<Role> _roleRepository;
        private readonly IEventBus _eventBus;
        private readonly IOptions<AppSettings> _appSetting;

        public AuthController(
            ILogger<AuthController> logger,
            IUnitOfWork unitOfWork,
            IBaseRepository<Account> accountRepository,
            IBaseRepository<Role> roleRepository,
            IEventBus eventBus,
            IOptions<AppSettings> appSetting
            )
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _accountRepository = accountRepository;
            _roleRepository = roleRepository;
            _eventBus = eventBus;
            _appSetting = appSetting;
        }

        [AllowAnonymous]
        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            var account = await _accountRepository.GetAll()
                .Include(x => x.AccountInRoles)
                .ThenInclude(x => x.Role)
                .FirstOrDefaultAsync(x => x.Username == loginDTO.Username);

            if (account == null)
            {
                throw new IdentityException("Username or password not match");
            }

            var isValid = PasswordUtilities.VerifyPassword(loginDTO.Password, account.PasswordSalt, account.PasswordHash);

            if (!isValid)
            {
                throw new IdentityException("Username or password not match");
            }

            var payload = new JwtPayload()
            {
                UserId = account.Id,
                Roles = account.AccountInRoles.Select(x => x.Role.Name).ToList()
            };

            var token = JwtHelper.GenerateToken(payload, _appSetting.Value.Jwt.Secret);

            return Ok(new ResponseModel()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Data = new { Token = token }
            });
        }

        [AllowAnonymous]
        [Route("test-publish")]
        [HttpGet]
        public async Task<IActionResult> TestPublish()
        {
            var event1 = new ProductSaledEvent()
            {
                ProductId = new Guid("ba2b8f70-aa6a-4c1b-8d34-66a41c812edc"),
                Quantity = 2
            };

            _eventBus.Publish("product_saled", event1);

            _logger.LogInformation("public event");

            await Task.CompletedTask;

            return Ok();
        }
    }
}
