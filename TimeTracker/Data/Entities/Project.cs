namespace TimeTracker.Data.Entities
{
    public class Project : BaseEntity
    {
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public virtual ICollection<Activity> Activities { get; set; }
        public Project()
        {
            Activities = new List<Activity>();
        }
    }
}
