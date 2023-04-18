using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TimeTracker.Data.Entities;

namespace TimeTracker.Data.Configurations
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.Property(e => e.Name).IsRequired().HasColumnType("nvarchar(100)");

            builder.Property(e => e.Sex).IsRequired().HasColumnType("nvarchar(6)");

            builder.Property(e => e.BirthDate).IsRequired();
        }
    }
}
