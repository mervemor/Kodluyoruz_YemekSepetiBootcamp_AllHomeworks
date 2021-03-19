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
    public class BestMovieController : ControllerBase
    {
        private readonly DatabaseContext _databaseContext;

        public BestMovieController(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        [HttpGet]
        public IActionResult Get()
        {
            BestMovie bestMovie = new BestMovie(_databaseContext);
            return Ok(bestMovie.GetList());
        }
    }
}