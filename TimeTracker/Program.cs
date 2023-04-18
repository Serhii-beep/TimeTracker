
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TimeTracker.Automapper;
using TimeTracker.Data;
using TimeTracker.Data.EFRepositories;
using TimeTracker.Data.Interfaces;
using TimeTracker.Services;

namespace TimeTracker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<TimeTrackerDbContext>(options => options.UseSqlServer(connectionString));

            builder.Services.AddControllers();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IMapper, Mapper>(implementationFactory =>
            {
                var profile = new AutoMapperProfile();
                var config = new MapperConfiguration(cfg => cfg.AddProfile(profile));
                return new(config);
            });
            builder.Services.AddScoped<ActivityTypeService>();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();

            //app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}