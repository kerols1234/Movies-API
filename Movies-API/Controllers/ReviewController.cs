using AngularAPI.Data;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    public class ReviewController : ControllerBase
    {
        private readonly AppDbContext _db;

        public ReviewController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult GetAllReviews()
        {
            return Ok(new { list = _db.Reviews.Include(obj => obj.Movie).ToList() });
        }

        [HttpGet]
        public IActionResult GetReviewById(int id)
        {
            return Ok(new { movie = _db.Reviews.Include(obj => obj.Movie).FirstOrDefault(obj => obj.Id == id) });
        }

        [HttpPost]
        public IActionResult post([FromBody] Review review)
        {
            Movies movies = _db.Movies.FirstOrDefault(obj => obj.Id == review.MovieId);
            if (movies == null)
            {
                return BadRequest("Wrong Movie id");
            }
            review.Movie = movies;
            _db.Reviews.Add(review);

            _db.SaveChanges();

            return Ok();
        }

        [HttpPut]
        public IActionResult put([FromBody] Review review)
        {
            Movies movies = _db.Movies.FirstOrDefault(obj => obj.Id == review.MovieId);
            if (movies == null)
            {
                return BadRequest("Wrong Movie id");
            }
            review.Movie = movies;
            if (!_db.Reviews.Any(obj => obj.Id == review.Id))
            {
                return BadRequest("Wrong id");
            }
            _db.Reviews.Update(review);
            _db.SaveChanges();

            return Ok();
        }

        [HttpDelete]
        public IActionResult delete(int id)
        {
            Review review = _db.Reviews.Find(id);
            if (review == null)
            {
                return BadRequest("Wrong id");
            }
            _db.Reviews.Remove(review);
            _db.SaveChanges();

            return Ok();
        }
    }
}