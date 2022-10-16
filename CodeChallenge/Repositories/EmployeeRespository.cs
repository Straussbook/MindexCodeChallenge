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
            //I had to change this from a singleOrDefault as it was returning null direct report lists
            return _employeeContext.Employees.Select(x => x).ToList().Where(e => e.EmployeeId == id).SingleOrDefault();
        }

        public Task SaveAsync()
        {
            return _employeeContext.SaveChangesAsync();
        }

        public Employee Remove(Employee employee)
        {
            return _employeeContext.Remove(employee).Entity;
        }

        public ReportingStructure GetStructure(Employee employee)
        {
            return new ReportingStructure
            {
                Employee = employee.EmployeeId,
                numberOfReports = ReportCount(employee)
            };
        }

        public int ReportCount(Employee employee)
        {
            if (employee.DirectReports == null)
                return 0;
            var count = employee.DirectReports.Count;
            foreach (var report in employee.DirectReports)
            {
                count += ReportCount(GetById(report.EmployeeId));
            }
            return count;
        }
    }
}
