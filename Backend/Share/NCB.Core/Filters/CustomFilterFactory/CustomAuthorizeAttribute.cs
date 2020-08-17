using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace NCB.Core.Filters.CustomFilterFactory
{
    public class CustomAuthorizeAttribute : Attribute, IFilterFactory
    {
        public bool IsReusable => false;

        public string[] Roles { get; set; }

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            var filter = serviceProvider.GetRequiredService<CustomAuthorizeFilter>();

            filter.Roles = Roles;

            return filter;
        }
    }
}
