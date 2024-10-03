using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CodeChallenge.Services;
using CodeChallenge.Models;

namespace CodeChallenge.Controllers
{
    [ApiController]
    [Route("api/compensation")]
    public class CompensationController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly ICompensationService _compensationService;

        public CompensationController(ILogger<CompensationController> logger, ICompensationService compensationService)
        {
            _logger = logger;
            _compensationService = compensationService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCompensat([FromBody] Compensation compensation)
        {
            if (compensation == null) 
            {
                return BadRequest();
            }

            if (!_compensationService.ValidateCompensation(compensation)) 
            {
                return BadRequest();
            }
            
            var comp = await _compensationService.CreateCompensation(compensation);

            return CreatedAtRoute("getEmployeeById", new { id = comp.Employee }, comp);
        }

        [HttpGet("{id}", Name = "getCompensationByEmployeeId")]
        public IActionResult GetCompensationByEmployeeId(string id)
        {
            if (string.IsNullOrEmpty(id)) 
            {
                return BadRequest();
            }

            var comp = _compensationService.GetByEmployeeId(id);

            return Ok(comp);
        }
    }
}
