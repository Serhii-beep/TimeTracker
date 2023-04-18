using TimeTracker.Data.Entities;
using TimeTracker.Data.Interfaces;

namespace TimeTracker.Data.EFRepositories
{
    public class ProjectRepository : EFRepository<Project>, IProjectRepository
    {
        public ProjectRepository(TimeTrackerDbContext context) : base(context) { }
    }
}
