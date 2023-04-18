namespace TimeTracker.Services.Dtos
{
    public class ActivityDto
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int ProjectId { get; set; }
        public int ActivityTypeId { get; set; }
        public int RoleId { get; set; }
    }
}
