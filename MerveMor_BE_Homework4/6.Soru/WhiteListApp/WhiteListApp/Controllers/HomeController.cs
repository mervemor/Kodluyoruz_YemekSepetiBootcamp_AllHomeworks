using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WhiteListApp.Attributes;

namespace WhiteListApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet("[action]")]
        [ServiceFilter(typeof(IpControlAttribute))]
        public IEnumerable<string> GetProducts()
        {
            return new List<string>
        {
             "pen",
             "mouse",
             "keyboard"
        };
        }

        [HttpGet("[action]")]
        public string GetProduct()
        {
            return "pen";
        }
    }
}