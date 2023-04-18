using TimeTracker.Data.Entities;
using TimeTracker.Data.Interfaces;

namespace TimeTracker.Data.EFRepositories
{
    public class ActivityTypeRepository : EFRepository<ActivityType>, IActivityTypeRepository
    {
        public ActivityTypeRepository(TimeTrackerDbContext context) : base(context) { }
    }
}
