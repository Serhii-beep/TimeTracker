using Microsoft.EntityFrameworkCore;
using TimeTracker.Data.Configurations;
using TimeTracker.Data.Entities;

namespace TimeTracker.Data
{
    public class TimeTrackerDbContext : DbContext
    {
        public virtual DbSet<Activity> Activities { get; set; }
        public virtual DbSet<ActivityType> ActivityTypes { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<Role> Roles { get; set; }

        public TimeTrackerDbContext(DbContextOptions<TimeTrackerDbContext> options) : base(options) 
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ActivityConfiguration());

            modelBuilder.ApplyConfiguration(new ActivityTypeConfiguration());

            modelBuilder.ApplyConfiguration(new EmployeeConfiguration());

            modelBuilder.ApplyConfiguration(new ProjectConfiguration());

            modelBuilder.ApplyConfiguration(new RoleConfiguration());
        }
    }
}
