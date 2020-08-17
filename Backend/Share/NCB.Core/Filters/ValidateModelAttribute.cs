using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NCB.Core.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NCB.Core.Filters
{
    public class ValidateModelAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.ModelState.IsValid)
            {
                await next();
            }

            var validationErrors = context.ModelState
                .Keys
                .SelectMany(k => context.ModelState[k].Errors)
                .Select(e => e.ErrorMessage)
                .ToArray();

            var json = new ResponseModel() { Data = validationErrors };
            context.Result = new BadRequestObjectResult(json);
        }
    }
}
