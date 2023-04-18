namespace TimeTracker.Data.Entities
{
    public class ActivityType : BaseEntity
    {
        public string Name { get; set; }
        public virtual ICollection<Activity> Activities { get; set; }
        public ActivityType()
        {
            Activities = new List<Activity>();
        }
    }
}
