using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WhiteListApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        [HttpGet("[action]")]
        public IEnumerable<string> GetCustomers()
        {
            return new List<string>
            {
             "Merve",
             "Mert"
            };
        }

        [HttpGet("[action]")]
        public string GetCustomer()
        {
            return "Merve";
        }
    }
}