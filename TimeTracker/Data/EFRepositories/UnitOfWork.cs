using TimeTracker.Data.Interfaces;

namespace TimeTracker.Data.EFRepositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TimeTrackerDbContext _context;

        public IActivityRepository ActivityRepository => new ActivityRepository(_context);

        public IActivityTypeRepository ActivityTypeRepository => new ActivityTypeRepository(_context);

        public IEmployeeRepository EmployeeRepository => new EmployeeRepository(_context);

        public IProjectRepository ProjectRepository => new ProjectRepository(_context);

        public IRoleRepository RoleRepository => new RoleRepository(_context);

        public UnitOfWork(TimeTrackerDbContext context)
        {
            _context = context;
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
