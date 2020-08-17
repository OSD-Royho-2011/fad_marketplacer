using System;
using System.Collections.Generic;
using System.Text;

namespace NCB.Core.Models
{
    public class JwtPayload
    {
        public Guid UserId { get; set; }

        public List<string> Roles { get; set; }
    }
}
