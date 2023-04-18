namespace TimeTracker.Data.Entities
{
    public class Activity : BaseEntity
    {
        public int EmployeeId { get; set; }
        public int ProjectId { get; set; }
        public int ActivityTypeId { get; set; }
        public int RoleId { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual Project Project { get; set; }
        public virtual ActivityType ActivityType { get; set; }
        public virtual Role Role { get; set; }
    }
}
