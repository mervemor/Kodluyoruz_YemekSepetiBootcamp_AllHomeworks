using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hotels.API.Controllers
{
    [Route("/[controller]")]
    //[Route("/v{version:apiVersion}/[controller]")] bu yöntemde versiyon bilgisi route parametresi olarak url’de taşınmaktadır.
    //query string versiyonlamaya nazaran, url fiziksel olarak versiyon bilgisiyle bütünleşiktir. Dolayısıyla yeni versiyon yayınlandığında client’ın elindeki url’i güncellemesi gerekecektir.
    [ApiController]
    [ApiVersion("1.0", Deprecated = true)]
    [ApiVersion("1.1")]
    [ApiVersion("2.0")]
    public class TestController : ControllerBase
    {
        [HttpGet(Name = nameof(GetCustomers))]
        public IActionResult GetCustomers()
        {
            List<string> customers = new List<string>()
            {
                "Merve Mor",
                "Mert Adatepe"
            };

            return Ok(customers);
        }

        //[ApiVersion("1.1", Deprecated = true)] yine bu şekilde buradada güncel ya da deprecated olan version belirtilebilir. 
        //[MapToApiVersion("1.1")] startup.cs de belirtmek yerine bu şekilde de metoda version verebiliriz. 
        [HttpGet(Name = nameof(GetCustomerV2))]
        public IActionResult GetCustomerV2()
        {
            List<string> customers = new List<string>()
            {
                "Merve ",
                "Mert "
            };

            return Ok(customers);
        }
    }
}