using CodeChallenge.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace CodeChallenge.Services;

public interface ICompensationService 
{
    bool ValidateCompensation (Compensation compensation) ;

    Task<Compensation> CreateCompensation(Compensation compensation);

    IEnumerable<Compensation> GetByEmployeeId(string employeeId);
}