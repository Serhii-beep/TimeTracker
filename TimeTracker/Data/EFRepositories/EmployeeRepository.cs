using TimeTracker.Data.Entities;
using TimeTracker.Data.Interfaces;

namespace TimeTracker.Data.EFRepositories
{
    public class EmployeeRepository : EFRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(TimeTrackerDbContext context) : base(context) { }
    }
}
