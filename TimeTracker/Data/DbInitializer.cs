namespace TimeTracker.Data
{
    public static class DbInitializer
    {
        public static void SeedData(TimeTrackerDbContext _context)
        {
            if(!_context.Employees.Any())
            {
                _context.Employees.Add(new()
                {
                    Name = "Serhii Peshko",
                    Sex = "Male",
                    BirthDate = DateTime.Parse("2002-08-31")
                });
                _context.SaveChanges();
            }
            if(!_context.ActivityTypes.Any())
            {
                _context.ActivityTypes.Add(new() { Name = "regular work" });
                _context.ActivityTypes.Add(new() { Name = "overtime" });
                _context.SaveChanges();
            }
            if(!_context.Roles.Any())
            {
                _context.Roles.Add(new() { Name = "Software Engineer" });
                _context.Roles.Add(new() { Name = "Software Architect" });
                _context.Roles.Add(new() { Name = "Team Lead" });
                _context.SaveChanges();
            }
            if(!_context.Projects.Any())
            {
                _context.Projects.Add(new()
                {
                    Name = "Project_1",
                    StartDate = DateTime.Parse("2022-06-26"),
                    EndDate = DateTime.Parse("2022-06-27")
                });
                _context.Projects.Add(new()
                {
                    Name = "Project_2",
                    StartDate = DateTime.Parse("2022-06-26"),
                    EndDate = DateTime.Parse("2022-06-27")
                });
                _context.Projects.Add(new()
                {
                    Name = "Project_3",
                    StartDate = DateTime.Parse("2022-06-25"),
                    EndDate = DateTime.Parse("2022-06-26")
                });
                _context.Projects.Add(new()
                {
                    Name = "Project_4",
                    StartDate = DateTime.Parse("2022-01-05"),
                    EndDate = DateTime.Parse("2022-01-06")
                });
                _context.SaveChanges();
            }
        }
    }
}
