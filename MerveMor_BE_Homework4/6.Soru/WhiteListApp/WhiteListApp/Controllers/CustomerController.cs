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
    public class CustomerController : ControllerBase
    {
        [HttpGet("[action]")]
        [ServiceFilter(typeof(IpControlAttribute))]
        public IEnumerable<string> GetCustomers()
        {
            return new List<string>
            {
             "Merve Mor",
             "Mert Adatepe"
            };
        }

        [HttpGet("[action]")]
        public string GetCustomer()
        {
            return "Merve Mor";
        }
    }
}