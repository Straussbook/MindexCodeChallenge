using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CodeChallenge.Services;
using CodeChallenge.Models;
using System.Net;

namespace CodeChallenge.Controllers
{
    [ApiController]
    [Route("api/employee")]
    public class EmployeeController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IEmployeeService _employeeService;
        private readonly ICompensationService _compensationService;

        public EmployeeController(ILogger<EmployeeController> logger, IEmployeeService employeeService, ICompensationService compensationService)
        {
            _logger = logger;
            _employeeService = employeeService;
            _compensationService = compensationService;
        }

        #region Employee

        [HttpPost]
        public IActionResult CreateEmployee([FromBody] Employee employee)
        {
            _logger.LogDebug($"Received employee create request for '{employee.FirstName} {employee.LastName}'");

            _employeeService.Create(employee);

            return CreatedAtRoute("getEmployeeById", new { id = employee.EmployeeId }, employee);
        }

        [HttpGet("{id}", Name = "getEmployeeById"), Route("")]
        public IActionResult GetEmployeeById(String id)
        {
            _logger.LogDebug($"Received employee get request for '{id}'");

            var employee = _employeeService.GetById(id);

            if (employee == null)
                return NotFound();

            return Ok(employee);
        }

        [HttpPut("{id}")]
        public IActionResult ReplaceEmployee(String id, [FromBody]Employee newEmployee)
        {
            _logger.LogDebug($"Recieved employee update request for '{id}'");

            var existingEmployee = _employeeService.GetById(id);
            if (existingEmployee == null)
                return NotFound();

            _employeeService.Replace(existingEmployee, newEmployee);

            return Ok(newEmployee);
        }

        #endregion

        #region Reporting Structures

        [HttpGet("Structure/{id}", Name = "getStructureById")]
        public IActionResult GetStructureById(String id)
        {
            _logger.LogDebug($"Received reporting structure request for '{id}'");

            var existingEmployee = _employeeService.GetById(id);
            if (existingEmployee == null)
                return NotFound();

            var structure = _employeeService.GetStructure(existingEmployee);

            return Ok(structure);
        }

        #endregion

        #region Compensation

        [HttpPost("Compensation/create")]
        public IActionResult CreateCompensation([FromBody]Compensation model)
        {
            _logger.LogDebug($"Received compensation create request for '{model.Employee}'");

            var existingEmployee = _employeeService.GetById(model.Employee);
            if (existingEmployee == null)
                return NotFound();

            if (_compensationService.GetById(model.Employee) != null)
                return Conflict();

            _compensationService.Create(model);

            return CreatedAtRoute("getCompensationById", new { id = model.Employee });
        }

        [HttpGet("Compensation/{id}", Name = "getCompensationById")]
        public IActionResult GetCompensationById(String id)
        {
            _logger.LogDebug($"Received compensation request for '{id}'");

            var existingEmployee = _employeeService.GetById(id);
            if (existingEmployee == null)
                return NotFound();

            var comp = _compensationService.GetById(id);
            if (comp == null)
                return NotFound();

            return Ok(comp);
        }

        #endregion

    }
}
