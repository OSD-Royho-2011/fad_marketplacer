using Microsoft.AspNetCore.Mvc.Authorization;
using System;
using System.Collections.Generic;
using System.Text;

namespace NCB.Core.Filters
{
    public class AllowAnonymousAttribute : Attribute, IAllowAnonymousFilter
    {

    }
}
