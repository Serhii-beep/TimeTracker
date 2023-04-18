namespace TimeTracker.Data.Entities
{
    public class Role : BaseEntity
    {
        public string Name { get; set; }
        public virtual ICollection<Activity> Activities { get; set; }
        public Role()
        {
            Activities = new List<Activity>();
        }
    }
}
