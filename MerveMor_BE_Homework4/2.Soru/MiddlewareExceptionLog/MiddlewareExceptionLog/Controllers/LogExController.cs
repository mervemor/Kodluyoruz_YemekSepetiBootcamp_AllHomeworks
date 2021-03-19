using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MiddlewareExceptionLog.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LogExController : ControllerBase
    {
        [HttpGet]
        public int Get([FromRoute]int a)
        {
            var b = 5 / a;
            return b;
        }
    }
}
