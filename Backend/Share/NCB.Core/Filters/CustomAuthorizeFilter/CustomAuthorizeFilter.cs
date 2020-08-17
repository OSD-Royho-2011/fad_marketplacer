using JWT.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NCB.Core.Helpers;
using NCB.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCB.Core.Filters.CustomAuthorizeFilter
{
    //public class CustomAuthorizeFilter : IAsyncAuthorizationFilter
    //{
    //    public string[] _roles;
    //    public readonly ILogger<CustomAuthorizeFilter> _logger;
    //    public readonly IOptions<AppSettings> _appSetting;

    //    public CustomAuthorizeFilter(ILogger<CustomAuthorizeFilter> logger, IOptions<AppSettings> appSetting, params string[] roles)
    //    {
    //        _roles = roles;
    //        _appSetting = appSetting;
    //        _logger = logger;
    //    }

    //    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    //    {
    //        var descriptor = context.ActionDescriptor as ControllerActionDescriptor;
    //        var actionName = descriptor.ActionName;
    //        var controllerName = descriptor.ControllerName;

    //        if (context.Filters.Any(item => item is IAllowAnonymousFilter))
    //        {
    //            return;
    //        }

    //        var accessToken = context.HttpContext.Request.Headers["x-access-token"].ToString();
    //        if (string.IsNullOrEmpty(accessToken))
    //        {
    //            context.Result = new UnauthorizedObjectResult(new ResponseModel() { Message = "x-access-token required" });
    //            return;
    //        }

    //        try
    //        {
    //            var a = JwtHelper.ValidateToken(accessToken, _appSetting.Value.Jwt.Secret);

    //            if (!_roles.Contains(a.Role))
    //            {
    //                context.Result = new UnauthorizedObjectResult(new ResponseModel() { Message = "UnAuthorized" });
    //            }
    //        }
    //        catch (TokenExpiredException e)
    //        {
    //            context.Result = new UnauthorizedObjectResult(new ResponseModel() { Message = e.Message });
    //        }
    //        catch (Exception)
    //        {
    //            context.Result = new UnauthorizedObjectResult(new ResponseModel() { Message = "InvalidToken" });
    //        }
    //        //var rp = context.HttpContext.RequestServices.GetService<IBaseRepository<Account, IdentityDbContext>>();
    //        //await rp.SaveAsync();

    //        await Task.CompletedTask;
    //    }
    //}
}
