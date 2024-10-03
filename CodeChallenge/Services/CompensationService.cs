using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeChallenge.Models;
using Microsoft.Extensions.Logging;
using CodeChallenge.Repositories;

namespace CodeChallenge.Services;

public class CompensationService : ICompensationService 
{
    private readonly ICompensationRepository _compensationRepository;
    private readonly ILogger<CompensationService> _logger;

    public CompensationService(ILogger<CompensationService> logger, ICompensationRepository compensationRepository)
    {
        _compensationRepository = compensationRepository;
        _logger = logger;
    }

    public bool ValidateCompensation (Compensation compensation) 
    {
        if (string.IsNullOrEmpty(compensation.Employee)) 
        {
            return false;
        }

        if (!_compensationRepository.EmployeeExists(compensation.Employee)) 
        {
            return false;
        }

        if (compensation.Salary == null || compensation.Salary <= 0) 
        {
            return false;
        }

        if (compensation.EffectiveDate == null) 
        {
            return false;
        }

        return true;
    }

    public async Task<Compensation> CreateCompensation(Compensation compensation) 
    {
        _compensationRepository.CreateCompensation(compensation);

        int rowsAdded = await _compensationRepository.SaveChangesAsync();
        if (rowsAdded != 1) 
        {
            _logger.LogError($"An unexpected number of rows were modified, {rowsAdded} rows were modified");
            throw new Exception("An error occurred while adding compensation: \n");
        }

        return compensation;
    }

    public IEnumerable<Compensation> GetByEmployeeId(string employeeId) 
    {
        return _compensationRepository.GetByEmployeeId(employeeId);
    }
}