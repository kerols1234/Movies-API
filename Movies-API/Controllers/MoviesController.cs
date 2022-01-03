using AngularAPI.Data;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Movies_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movies_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [EnableCors]
    public class MoviesController : ControllerBase
    {
        private readonly AppDbContext _db;

        public MoviesController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult GetAllMovies()
        {
            return Ok(new { list = _db.Movies.ToList() });
        }

        [HttpGet]
        public IActionResult GetMovieById(int id)
        {
            return Ok(new { movie = _db.Movies.FirstOrDefault(obj => obj.Id == id) });
        }

        [HttpPost]
        public IActionResult post([FromBody] Movies movies)
        {
            _db.Movies.Add(movies);
            _db.SaveChanges();

            return Ok();
        }

        [HttpPut]
        public IActionResult put([FromBody] Movies movies)
        {
            if (!_db.Movies.Any(obj => obj.Id == movies.Id))
            {
                return BadRequest("Wrong id");
            }
            var result = _db.Movies.Update(movies);
            _db.SaveChanges();

            return Ok();
        }

        [HttpDelete]
        public IActionResult delete(int id)
        {
            Movies movies = _db.Movies.Find(id);
            if (movies == null)
            {
                return BadRequest("Wrong id");
            }
            _db.Movies.Remove(movies);
            _db.SaveChanges();

            return Ok();
        }
    }
}