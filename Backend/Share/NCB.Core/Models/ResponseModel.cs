using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace NCB.Core.Models
{
    public class ResponseModel
    {
        public HttpStatusCode StatusCode { get; set; }
        public object Data { get; set; }
        public string Message { get; set; }
    }
}
