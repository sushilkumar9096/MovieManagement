using Microsoft.AspNetCore.Mvc;
using MovieManagement.Domain.Entities;
using MovieManagement.Domain.Repositories;
using System.Collections.Generic;

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

        [HttpGet]
        public ActionResult<IEnumerable<Actor>> GetActors()
        {
            var actors = _unitOfWork.Actors.GetAll();
            return Ok(actors);
        }
    }
}
