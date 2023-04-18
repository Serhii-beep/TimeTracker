using TimeTracker.Data.Entities;
using TimeTracker.Data.Interfaces;

namespace TimeTracker.Data.EFRepositories
{
    public class RoleRepository : EFRepository<Role>, IRoleRepository
    {
        public RoleRepository(TimeTrackerDbContext context) : base(context) { }
    }
}
