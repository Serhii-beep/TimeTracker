using TimeTracker.Data.Entities;
using TimeTracker.Data.Interfaces;

namespace TimeTracker.Data.EFRepositories
{
    public class ActivityRepository : EFRepository<Activity>, IActivityRepository
    {
        public ActivityRepository(TimeTrackerDbContext context) : base(context) { }
    }
}
