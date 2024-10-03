using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeChallenge.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using CodeChallenge.Data;

namespace CodeChallenge.Repositories;

public class CompensationRepository : ICompensationRepository 
{
    private readonly CompensationContext _compensationContext;

    private readonly EmployeeContext _employeeContext;

    private readonly ILogger<CompensationRepository> _logger;

    public CompensationRepository(ILogger<CompensationRepository> logger, CompensationContext compensationContext, EmployeeContext employeeContext)
    {
        _compensationContext = compensationContext;
        _employeeContext = employeeContext;
        _logger = logger;
    }

    public IEnumerable<Compensation> GetByEmployeeId(string employeeId) 
    {
        return _compensationContext.Compensations.Where(x => x.Employee == employeeId);
    }

    public void CreateCompensation(Compensation compensation) 
    {
        compensation.Id = Guid.NewGuid().ToString();
        _compensationContext.Compensations.Add(compensation);
    }

    public bool EmployeeExists(string employeeId) 
    {
        return _employeeContext.Employees.Any(x => x.EmployeeId == employeeId);
    }

    public Task<int> SaveChangesAsync() 
    {
        return _compensationContext.SaveChangesAsync();
    }
}