using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.OpenApi.Models;
using NCB.Core.Filters.CustomAuthorizeFilter;
using NCB.Core.Filters.CustomFilterFactory;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NCB.Core.Filters
{
    public class AccessTokenHeaderParameterOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var filterPipeline = context.ApiDescription.ActionDescriptor.FilterDescriptors;
            //var isAuthorized = filterPipeline.Select(filterInfo => filterInfo.Filter).Any(filter => filter is CustomAuthorizeFilterAttribute);
            var isAuthorized = filterPipeline.Select(filterInfo => filterInfo.Filter).Any(filter => filter is CustomAuthorizeAttribute);
            var allowAnonymous = filterPipeline.Select(filterInfo => filterInfo.Filter).Any(filter => filter is IAllowAnonymousFilter);
            if (isAuthorized && !allowAnonymous)
            {
                if (operation.Parameters == null)
                {
                    operation.Parameters = new List<OpenApiParameter>();
                }

                operation.Parameters.Add(new OpenApiParameter()
                {
                    Name = "x-access-token",
                    In = ParameterLocation.Header,
                    Description = "Access Token",
                    Required = true,
                    Schema = new OpenApiSchema
                    {
                        Type = "string"
                    }
                });
            }
        }
    }
}
