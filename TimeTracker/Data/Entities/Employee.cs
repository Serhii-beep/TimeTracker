namespace TimeTracker.Data.Entities
{
    public class Employee : BaseEntity
    {
        public string Name { get; set; }
        public string Sex { get; set; }
        public DateTime BirthDate { get; set; }
        public virtual ICollection<Activity> Activities { get; set; }
        public Employee()
        {
            Activities = new List<Activity>();
        }
    }
}
