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
    public class GenreController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public GenreController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/Genre
        [HttpGet]
        public ActionResult<IEnumerable<GenreDto>> GetGenres()
        {
            var genres = _unitOfWork.Genres.GetAll();
            var dtos = genres.Select(g => new GenreDto
            {
                Id = g.Id,
                Name = g.Name
            }).ToList();
            return Ok(dtos);
        }

        // POST: api/Genre
        [HttpPost]
        public ActionResult<GenreDto> CreateGenre([FromBody] GenreDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Name))
            {
                return BadRequest("Invalid genre name");
            }

            var genre = new Genre
            {
                Name = dto.Name
            };

            _unitOfWork.Genres.Add(genre);
            _unitOfWork.Save();

            dto.Id = genre.Id;
            return CreatedAtAction(nameof(GetGenres), new { id = genre.Id }, dto);
        }
    }
}
