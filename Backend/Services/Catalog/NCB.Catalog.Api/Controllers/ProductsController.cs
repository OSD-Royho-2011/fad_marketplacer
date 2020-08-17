using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NCB.Catalog.Api.Application.Commands;
using NCB.Catalog.Api.Application.DTOs;
using NCB.Catalog.Api.Application.Queries;
using NCB.Core.Filters;
using NCB.Core.Filters.CustomFilterFactory;
using NCB.Core.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace NCB.Catalog.Api.Controllers
{
    [Route("api/catalogs/products")]
    //[ValidateActionParameters]
    [CustomAuthorize(Roles = new string[] { "Admin", "Normal" })]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IMediator _mediator;
        public ProductsController(
            ILogger<ProductsController> logger,
            IMediator mediator
            )
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll([FromQuery, Required] ProductGetListQuery request)
        {
            var a = await _mediator.Send(request);
            return Ok(new ResponseModel() { Data = a });
        }

        [Route("{productId}")]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] ProductUpdateDTO request, [FromRoute] Guid productId)
        {
            var res = await _mediator.Send(new ProductUpdateCommand() { ProductId = productId, Quantity = request.Quantity });

            //test again
            if (!res)
            {
                return BadRequest();
            };

            return Ok(new ResponseModel() { StatusCode = System.Net.HttpStatusCode.OK, Message = "Update successfully" });
        }
    }
}
