using System;
using System.Threading.Tasks;
using AutoMapper;
using CarRentalApi.Common;
using CarRentalApi.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CarRentalApi.Controllers
{
    [Route("api/Camps")]
    public class CampsController : Controller
    {
        private readonly ICampRepository _reposetory;
        private readonly ILogger<CampsController> _logger;
        private readonly IMapper _mapper;

        public CampsController(ICampRepository repository , IMapper mapper , ILogger<CampsController> logger)
        {
            _mapper = mapper;
            _reposetory = repository;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var camps = _reposetory.GetAllCamps();
            if(camps == null)
                return NotFound("Camps not found");
            return Ok(camps);
        }

        [HttpGet("{id}" , Name = "newCamp")]
        public IActionResult Get(int id , bool includeSpeaker = false)
        {
            var camp = includeSpeaker ? _reposetory.GetCampWithSpeakers(id) : _reposetory.GetCamp(id);

            if(camp == null)
                return NotFound("camp not found ");
            return Ok(camp);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Camp model)
        {
            _reposetory.Add(model);
            if(await _reposetory.SaveAllAsync())
            {
                var newUri = Url.Link("newCamp" , new { id = model.Id });
                return Created(newUri , model);
            }
            return BadRequest($"This is bad BadRequest");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> put(int id , [FromBody] Camp model)
        {
            try
            {
                var oldCamp = _reposetory.GetCamp(id);
                if(oldCamp == null)
                    return NotFound($"Could not found {id}");
                oldCamp.Name = model.Name ?? oldCamp.Name;
                oldCamp.Description = model.Description ?? oldCamp.Description;
                oldCamp.Location = model.Location ?? oldCamp.Location;
                oldCamp.Length = model.Length > 0 ? model.Length : oldCamp.Length;
                oldCamp.EventDate = model.EventDate != DateTime.MinValue ? model.EventDate : oldCamp.EventDate;
                if(await _reposetory.SaveAllAsync())
                {
                    return Ok(oldCamp);
                }
            }
            catch(System.Exception)
            {
                _logger.LogError("Could not update Camp");
            }
            return BadRequest("Couldn't update camp");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deleteCamp = _reposetory.GetCamp(id);
                if(deleteCamp == null)
                    return NotFound($"camp with {id} Not found ");
                _reposetory.Delete(deleteCamp);
                if(await _reposetory.SaveAllAsync())
                {
                    return Ok();
                }
            }
            catch(Exception e)
            {
                _logger.LogError("camp can't delete ");
            }
            return BadRequest();
        }
    }
}
