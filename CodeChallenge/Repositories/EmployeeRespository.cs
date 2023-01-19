using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeChallenge.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using CodeChallenge.Data;

namespace CodeChallenge.Repositories
{
    public class EmployeeRespository : IEmployeeRepository
    {
        private readonly EmployeeContext _employeeContext;
        private readonly ILogger<IEmployeeRepository> _logger;

        public EmployeeRespository(ILogger<IEmployeeRepository> logger, EmployeeContext employeeContext)
        {
            _employeeContext = employeeContext;
            _logger = logger;
        }

        public Employee Add(Employee employee)
        {
            employee.EmployeeId = Guid.NewGuid().ToString();
            _employeeContext.Employees.Add(employee);
            return employee;
        }

        public Employee GetById(string id)
        { 
            return _employeeContext.Employees.SingleOrDefault(e => e.EmployeeId == id);
        }

        public int GetByIdWithReportsCount(string id, out Employee employee)
		{
            employee = GetById(id);
            return CountDirectReports(employee);
		}

        //Use a breadth first search to load employees in DirectReports and return the total number visited
        public int CountDirectReports(Employee employee)
		{
            if(employee == null) 
                return 0;

            Queue<Employee> employees = new();
            HashSet<string> visited = new();
            employees.Enqueue(employee);
            //load DirectReports of the starting employee
            _employeeContext.Entry(employee).Collection(x => x.DirectReports).Load();

            while(employees.Count > 0)
            {
                var current = employees.Dequeue();
                if(current.DirectReports != null)
				{
                    foreach(Employee e in current.DirectReports)
					{
						if(!visited.Contains(e.EmployeeId))
						{
							employees.Enqueue(e);
							visited.Add(e.EmployeeId);

                            _employeeContext.Entry(e).Collection(a => a.DirectReports).Load();
                        }
					}
				}
            }
            return visited.Count;
        }

        public Task SaveAsync()
        {
            return _employeeContext.SaveChangesAsync();
        }

        public Employee Remove(Employee employee)
        {
            return _employeeContext.Remove(employee).Entity;
        }
    }
}
