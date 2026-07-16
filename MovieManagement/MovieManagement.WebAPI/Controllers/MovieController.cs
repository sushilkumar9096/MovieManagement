using Microsoft.AspNetCore.Mvc;
using MovieManagement.Domain.Entities;
using MovieManagement.Domain.Repositories;
using MovieManagement.WebAPI.DTOs;
using System.Collections.Generic;
using System.Linq;

namespace MovieManagement.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovieController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public MovieController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/Movie
        [HttpGet]
        public ActionResult<IEnumerable<MovieDto>> GetMovies()
        {
            var movies = _unitOfWork.Movies.GetAll();
            var dtos = movies.Select(MapToDto).ToList();
            return Ok(dtos);
        }

        // GET: api/Movie/5
        [HttpGet("{id}")]
        public ActionResult<MovieDto> GetMovie(int id)
        {
            var movie = _unitOfWork.Movies.GetById(id);
            if (movie == null)
            {
                return NotFound($"Movie with ID {id} not found.");
            }
            return Ok(MapToDto(movie));
        }

        // POST: api/Movie
        [HttpPost]
        public ActionResult<MovieDto> CreateMovie([FromBody] MovieCreateDto dto)
        {
            if (dto == null) return BadRequest("Invalid client request");

            // Validate Actor
            var actor = _unitOfWork.Actors.GetById(dto.ActorId);
            if (actor == null)
            {
                return BadRequest($"Actor with ID {dto.ActorId} does not exist.");
            }

            var movie = new Movie
            {
                Name = dto.Name,
                Description = dto.Description,
                ActorId = dto.ActorId,
                Actor = actor
            };

            // Link Genres
            if (dto.GenreIds != null && dto.GenreIds.Count > 0)
            {
                var genres = _unitOfWork.Genres.Find(g => dto.GenreIds.Contains(g.Id)).ToList();
                movie.Genre = genres;
            }

            _unitOfWork.Movies.Add(movie);
            _unitOfWork.Save();

            // Fetch to ensure loaded links
            var savedMovie = _unitOfWork.Movies.GetById(movie.Id);
            return CreatedAtAction(nameof(GetMovie), new { id = movie.Id }, MapToDto(savedMovie!));
        }

        // DELETE: api/Movie/5
        [HttpDelete("{id}")]
        public IActionResult DeleteMovie(int id)
        {
            var movie = _unitOfWork.Movies.GetById(id);
            if (movie == null)
            {
                return NotFound($"Movie with ID {id} not found.");
            }

            _unitOfWork.Movies.Remove(movie);
            _unitOfWork.Save();
            return NoContent();
        }

        private static MovieDto MapToDto(Movie movie)
        {
            return new MovieDto
            {
                Id = movie.Id,
                Name = movie.Name,
                Description = movie.Description,
                ActorId = movie.ActorId,
                ActorName = movie.Actor != null ? $"{movie.Actor.FirstName} {movie.Actor.LastName}" : "Unknown",
                Genres = movie.Genre != null ? movie.Genre.Select(g => g.Name).ToList() : new List<string>()
            };
        }
    }
}
