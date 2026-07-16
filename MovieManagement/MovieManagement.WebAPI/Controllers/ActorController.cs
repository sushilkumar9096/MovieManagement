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
    public class ActorController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ActorController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/Actor
        [HttpGet]
        public ActionResult<IEnumerable<ActorDto>> GetActors()
        {
            var actors = _unitOfWork.Actors.GetAll();
            var dtos = actors.Select(MapToDto).ToList();
            return Ok(dtos);
        }

        // GET: api/Actor/5
        [HttpGet("{id}")]
        public ActionResult<ActorDto> GetActor(int id)
        {
            var actor = _unitOfWork.Actors.GetById(id);
            if (actor == null)
            {
                return NotFound($"Actor with ID {id} not found.");
            }
            return Ok(MapToDto(actor));
        }

        // POST: api/Actor
        [HttpPost]
        public ActionResult<ActorDto> CreateActor([FromBody] ActorCreateDto dto)
        {
            if (dto == null) return BadRequest("Invalid client request");

            var actor = new Actor
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName
            };

            if (!string.IsNullOrWhiteSpace(dto.BiographyDescription))
            {
                actor.Biography = new Biography
                {
                    Description = dto.BiographyDescription,
                    Actor = actor
                };
            }

            _unitOfWork.Actors.Add(actor);
            _unitOfWork.Save();

            // Fetch to ensure relations are loaded for response
            var savedActor = _unitOfWork.Actors.GetById(actor.Id);
            return CreatedAtAction(nameof(GetActor), new { id = actor.Id }, MapToDto(savedActor!));
        }

        // PUT: api/Actor/5
        [HttpPut("{id}")]
        public IActionResult UpdateActor(int id, [FromBody] ActorCreateDto dto)
        {
            if (dto == null) return BadRequest("Invalid client request");

            var actor = _unitOfWork.Actors.GetById(id);
            if (actor == null)
            {
                return NotFound($"Actor with ID {id} not found.");
            }

            actor.FirstName = dto.FirstName;
            actor.LastName = dto.LastName;

            if (actor.Biography == null)
            {
                if (!string.IsNullOrWhiteSpace(dto.BiographyDescription))
                {
                    actor.Biography = new Biography
                    {
                        Description = dto.BiographyDescription,
                        Actor = actor
                    };
                }
            }
            else
            {
                actor.Biography.Description = dto.BiographyDescription ?? string.Empty;
            }

            _unitOfWork.Save();
            return NoContent();
        }

        // DELETE: api/Actor/5
        [HttpDelete("{id}")]
        public IActionResult DeleteActor(int id)
        {
            var actor = _unitOfWork.Actors.GetById(id);
            if (actor == null)
            {
                return NotFound($"Actor with ID {id} not found.");
            }

            _unitOfWork.Actors.Remove(actor);
            _unitOfWork.Save();
            return NoContent();
        }

        private static ActorDto MapToDto(Actor actor)
        {
            return new ActorDto
            {
                Id = actor.Id,
                FirstName = actor.FirstName,
                LastName = actor.LastName,
                BiographyDescription = actor.Biography?.Description ?? string.Empty,
                Movies = actor.Movies.Select(m => new MovieDto
                {
                    Id = m.Id,
                    Name = m.Name,
                    Description = m.Description,
                    ActorId = m.ActorId,
                    ActorName = $"{actor.FirstName} {actor.LastName}",
                    Genres = m.Genre.Select(g => g.Name).ToList()
                }).ToList()
            };
        }
    }
}
