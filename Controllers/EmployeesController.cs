using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CarRentalApi.Common;
using CarRentalApi.Entities;
using CarRentalApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CarRentalApi.Controllers
{
    [Route("api/Employees")]
    public class EmployeesController : Controller
    {
        private readonly ICampRepository _reposetory;
        private readonly ILogger<CampsController> _logger;
        private readonly IMapper _mapper;

        public EmployeesController(ICampRepository repository , IMapper mapper , ILogger<CampsController> logger)
        {
            _mapper = mapper;
            _reposetory = repository;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var employee = _reposetory.GetAllEmployees();

                return Ok(_mapper.Map<IEnumerable<EmployeeModel>>(employee));
            }
            catch(Exception)
            {
                _logger.LogError("Failed to execute GET");
                return BadRequest();
            }
        }

        [HttpGet("{id}" , Name = "newEmployee")]
        public IActionResult Get(int id)
        {
            var employee = _reposetory.GetEmployee(id);

            if(employee == null)
                return NotFound($"employee not found {id}");
            return Ok(employee);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Employee model)
        {
            try
            {
                _reposetory.Add(model);
                if(await _reposetory.SaveAllAsync())
                {
                    var newEmployee = Url.Link("newEmployee" , new { id = model.Id });
                    return Created(newEmployee , model);
                }
            }
            catch(Exception)
            {
                _logger.LogError("Failed to execute POST");
            }
            return BadRequest();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id , [FromBody] Employee model)
        {
            try
            {
                var employee = _reposetory.GetEmployee(id);

                if(employee == null)
                    return NotFound($"employee not found {id}");
                employee.FirstName = model.FirstName ?? employee.FirstName;
                employee.LastName = model.LastName ?? employee.LastName;
                employee.Gender = model.Gender ?? employee.Gender;
                employee.Salary = model.Salary > 0 ? model.Salary : employee.Salary;
                if(await _reposetory.SaveAllAsync())
                {
                    return Ok(employee);
                }
                return BadRequest();
            }
            catch(Exception)
            {
                _logger.LogError("Failed to execute PUT");
                return BadRequest();
            }
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            try
            {
                return Ok();
            }
            catch(Exception)
            {
                _logger.LogError("Failed to execute DELETE");
                return BadRequest();
            }
        }
    }
}
