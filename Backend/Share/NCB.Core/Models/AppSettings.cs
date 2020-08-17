using System;
using System.Collections.Generic;
using System.Text;

namespace NCB.Core.Models
{
    public class AppSettings
    {
        public Jwt Jwt { get; set; }
    }

    public class Jwt
    {
        public string Secret { get; set; }
    }
}
