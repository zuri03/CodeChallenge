using CodeChallenge.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace CodeChallenge.Repositories
{
    public interface ICompensationRepository
    {
        IEnumerable<Compensation> GetByEmployeeId(string employeeId);
        void CreateCompensation(Compensation compensation);
        bool EmployeeExists(string employeeId);
        Task<int> SaveChangesAsync();
    }
}