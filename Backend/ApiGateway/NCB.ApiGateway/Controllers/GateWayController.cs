using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NCB.ApiGateway.Controllers
{
    [Route("gatewaycustom")]
    public class GateWayController : ControllerBase
    {
        public GateWayController()
        {

        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {

            return Ok("abc");
        }
    }
}
