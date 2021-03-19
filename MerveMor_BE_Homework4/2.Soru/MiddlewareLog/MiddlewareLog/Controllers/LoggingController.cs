using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MiddlewareLog.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoggingController : ControllerBase
    {
        private readonly ILogger _logger;

        public LoggingController(ILogger<LoggingController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Student>> Get()
        {
            _logger.LogInformation("Kaydetme başladı {ID}");

            List<Student> list = new List<Student>();
            list.Add(new Student() { Id = 1, FullName = "Merve Mor1" });
            list.Add(new Student() { Id = 2, FullName = "Merve Mor2" });

            _logger.LogInformation("Kaydetme tamamlandı {ID}", list);
            return list.ToList();
        }
    }

    public class Student
    {
        public int Id { get; set; }
        public string FullName { get; set; }
    }


}
