using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Week2_Homework2.Business;
using Week2_Homework2.Data.Context;
using Week2_Homework2.Data.Entity;
using Week2_Homework2.RequestModels;

namespace Week2_Homework2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly DatabaseContext _databaseContext;

        public MovieController(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_databaseContext.Movie.ToList());
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var result = _databaseContext.Movie.FirstOrDefault(x => x.MovieId == id);
            return Ok(result);
        }

        [HttpPost]
        
        public IActionResult Post([FromBody] MovieRequest movieRequest)
        {
            
            if(ModelState.IsValid)
            {
                Movie movie = new Movie();
                movie.MovieName = movieRequest.MovieName;
                movie.MovieScore = movieRequest.MovieScore;
                movie.UserId = movieRequest.UserId;
                _databaseContext.Movie.Add(movie);
                _databaseContext.SaveChanges();
                MovieTypeRelation movieTypeRelation = new MovieTypeRelation();
                movieTypeRelation.MovieId = movie.MovieId;
                movieTypeRelation.MovieTypeId = movieRequest.MovieTypeId;
                _databaseContext.MovieTypeRelation.Add(movieTypeRelation);
                _databaseContext.SaveChanges();
                return Ok(movie);
            }


            return BadRequest();
           
        }

        [HttpPut]
        public IActionResult Put([FromBody] MovieRequest movieRequest) 
        {
            return Ok();
        }


        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var deletedMovie = _databaseContext.Movie.FirstOrDefault(x => x.MovieId == id);
                _databaseContext.Movie.Remove(deletedMovie);
                var movieTypeRelation = _databaseContext.MovieTypeRelation.Where(x => x.MovieId == id);
                _databaseContext.MovieTypeRelation.RemoveRange(movieTypeRelation);
                _databaseContext.SaveChanges();
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
            
        }
    }
}