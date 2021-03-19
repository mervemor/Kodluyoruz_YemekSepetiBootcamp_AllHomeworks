using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Week2_Homework2.Data.Context;
using Week2_Homework2.Data.Entity;
using Week2_Homework2.RequestModels;

namespace Week2_Homework2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieTypeController : ControllerBase
    {
        private readonly DatabaseContext _databaseContext;

        public MovieTypeController(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_databaseContext.MovieType.ToList());
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var result = _databaseContext.MovieType.FirstOrDefault(x => x.MovieTypeId == id);
            return Ok(result);
        }

        [HttpPost]
        public IActionResult Post([FromBody] MovieTypeRequest movieTypeRequest)
        {
            if(ModelState.IsValid)
            {
                MovieType movieType = new MovieType();
                movieType.Name = movieTypeRequest.Name;
                _databaseContext.MovieType.Add(movieType);
                _databaseContext.SaveChanges();
                return Ok(movieType);
            }

            return BadRequest();

        }


    }
}