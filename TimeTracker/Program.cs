
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
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
            Log.Logger = new LoggerConfiguration().MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            var builder = WebApplication.CreateBuilder(args);

            builder.Host.UseSerilog();

            // Add services to the container.

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<TimeTrackerDbContext>(options => options.UseSqlServer(connectionString)
                .LogTo(Log.Information, new[] { DbLoggerCategory.Database.Command.Name }, LogLevel.Information));

            builder.Services.AddControllers();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IMapper, Mapper>(implementationFactory =>
            {
                var profile = new AutoMapperProfile();
                var config = new MapperConfiguration(cfg => cfg.AddProfile(profile));
                return new(config);
            });
            builder.Services.AddScoped<ActivityTypeService>();
            builder.Services.AddScoped<ActivityService>();
            builder.Services.AddScoped<EmployeeService>();
            builder.Services.AddScoped<ProjectService>();
            builder.Services.AddScoped<RoleService>();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            app.UseSerilogRequestLogging(opts =>
            {
                opts.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
                {
                    var routeData = httpContext.GetRouteData();
                    var controller = routeData.Values["controller"];
                    var action = routeData.Values["action"];
                    diagnosticContext.Set("Controller", controller);
                    diagnosticContext.Set("Action", action);
                };
            });

            app.UseSwagger();
            app.UseSwaggerUI();

            //app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}