using AutoMapper;
using TimeTracker.Data.Entities;
using TimeTracker.Services.Dtos;

namespace TimeTracker.Automapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() 
        {
            CreateMap<Activity, ActivityDto>().ReverseMap();
            CreateMap<ActivityType, ActivityTypeDto>().ReverseMap();
            CreateMap<Employee, EmployeeDto>().ReverseMap();
            CreateMap<Project, ProjectDto>().ReverseMap();
            CreateMap<Role,  RoleDto>().ReverseMap();
        }
    }
}
