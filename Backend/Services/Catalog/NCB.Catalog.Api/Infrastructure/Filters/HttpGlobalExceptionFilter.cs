using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NCB.Catalog.Api.Infrastructure.Exceptions;
using NCB.Core.Infrastructure.ActionResults;
using NCB.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NCB.Catalog.Api.Infrastructure.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly IWebHostEnvironment _env;

        private readonly ILogger<GlobalExceptionFilter> _logger;

        public GlobalExceptionFilter(IWebHostEnvironment env, ILogger<GlobalExceptionFilter> logger)
        {
            _env = env;
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            //context.Result = new JsonResult("value");
            if (context.Exception.GetType() == typeof(CatalogException))
            {
                context.Result = new BadRequestObjectResult(new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = context.Exception.Message.ToString()
                });
            }
            else
            {
                if (_env.IsDevelopment())
                {
                    context.Result = new InternalServerErrorObjectResult(context.Exception);
                }
                else
                {
                    context.Result = new InternalServerErrorObjectResult("-- 500 Internal Server Error --");
                }
            }

            context.ExceptionHandled = true;
        }
    }
}
