using CodeChallenge.Models;
using Microsoft.Extensions.Logging;
using CodeChallenge.Repositories;

namespace CodeChallenge.Services
{
    public class ReportingStructureService : IReportingStructureService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<ReportingStructureService> _logger;

        public ReportingStructureService(ILogger<ReportingStructureService> logger, IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
        }

        public ReportingStructure GetById(string id)
        {
            if(!string.IsNullOrEmpty(id))
            {
                var count = _employeeRepository.GetByIdWithReportsCount(id, out Employee employee);

                if(employee == null)
                    return null;

                return new ReportingStructure()
                {
                    Employee = employee,
                    NumberOfReports = count
                };
			}

            return null;
        }
    }
}
