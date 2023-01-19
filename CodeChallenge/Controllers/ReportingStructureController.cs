using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CodeChallenge.Services;

namespace CodeChallenge.Controllers
{
    [ApiController]
    [Route("api/numberOfReports")]
    public class ReportingStructureController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IReportingStructureService _reportingStructureService;

        public ReportingStructureController(ILogger<ReportingStructureController> logger, IReportingStructureService reportingStructureService)
        {
            _logger = logger;
            _reportingStructureService = reportingStructureService;
        }

        [HttpGet("{id}", Name = "getReportingStructureById")]
        public IActionResult GetReportingStructureById(String id)
        {
            _logger.LogDebug($"Received reportingStructure get request for '{id}'");

            var reportingStructure = _reportingStructureService.GetById(id);

            if(reportingStructure == null)
                return NotFound();

            return Ok(reportingStructure);
        }
    }
}
