using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Week2_Homework2.Business;
using Week2_Homework2.Data.Context;

namespace Week2_Homework2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessController : ControllerBase
    {

        private readonly DatabaseContext _databaseContext;

        public BusinessController(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        [HttpGet]
        public IActionResult Get()
        {
            MovieDetail movieDetail = new MovieDetail(_databaseContext);
            return Ok(movieDetail.MovieItemList);
        }
    }
}