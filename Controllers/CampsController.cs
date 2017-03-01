using AutoMapper;
using CarRentalApi.Common;
using CarRentalApi.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalApi.Controllers
{
    [Route("api/Camps")]
    public class CampsController : Controller
    {
        private readonly ICampRepository _reposetory;

        public CampsController(ICampRepository repository,IMapper mapper)
        {

            _reposetory = repository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var camps = _reposetory.GetAllCamps();
            if(camps == null)
                return NotFound("Camps not found");
                return Ok(camps);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id , bool includeSpeaker = false)
        {
            var camp = includeSpeaker ? _reposetory.GetCampWithSpeakers(id) : _reposetory.GetCamp(id);

            if(camp == null)
                return NotFound("camp not found ");
            return Ok(camp);
        }

        [HttpPost]
        public IActionResult Get([FromBody] Camp model)
        {
            _reposetory.Add(model);
            if(_reposetory.SaveAll())
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}
