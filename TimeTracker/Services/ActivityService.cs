using AutoMapper;
using Microsoft.VisualBasic;
using System.Text;
using TimeTracker.Data.Entities;
using TimeTracker.Data.Interfaces;
using TimeTracker.Services.Dtos;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TimeTracker.Services
{
    public class ActivityService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ActivityService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseModel<ActivityDto>> CreateActivityAsync(ActivityDto activity)
        {
            var checkCompleteness = await CheckDataCompleteness(activity);
            if(!checkCompleteness.Item1)
            {
                return ResponseModel<ActivityDto>.Failure(StatusCodes.Status404NotFound, checkCompleteness.Item2);
            }
            var activityEntity = _mapper.Map<Activity>(activity);
            try
            {
                await _unitOfWork.ActivityRepository.AddAsync(activityEntity);
                await _unitOfWork.SaveAsync();
            }
            catch(Exception ex)
            {
                return ResponseModel<ActivityDto>.Failure(StatusCodes.Status500InternalServerError, $"Creation process failed. Message: {ex.Message}");
            }
            return ResponseModel<ActivityDto>.Success(_mapper.Map<ActivityDto>(activityEntity));
        }

        public async Task<ResponseModel<bool>> DeleteActivityByIdAsync(int id)
        {
            var activity = await _unitOfWork.ActivityRepository.GetByIdAsync(id);
            if(activity == null)
            {
                return ResponseModel<bool>.Failure(StatusCodes.Status404NotFound, $"Activity with id = {id} does not exist");
            }
            try
            {
                await _unitOfWork.ActivityRepository.DeleteByIdAsync(id);
                await _unitOfWork.SaveAsync();
            }
            catch(Exception ex)
            {
                return ResponseModel<bool>.Failure(StatusCodes.Status500InternalServerError, $"Deletion process failed. Message: {ex.Message}");
            }
            return ResponseModel<bool>.Success(true);
        }

        public async Task<ResponseModel<IEnumerable<ActivityDto>>> GetAllActivitiesAsync()
        {
            var activities = await _unitOfWork.ActivityRepository.GetAllAsync();
            return ResponseModel<IEnumerable<ActivityDto>>.Success(_mapper.Map<IEnumerable<ActivityDto>>(activities));
        }

        public async Task<ResponseModel<ActivityDto>> GetActivityById(int id)
        {
            var activity = await _unitOfWork.ActivityRepository.GetByIdAsync(id);
            if(activity == null)
            {
                return ResponseModel<ActivityDto>.Failure(StatusCodes.Status404NotFound, $"Activity with id = {id} does not exist");
            }
            return ResponseModel<ActivityDto>.Success(_mapper.Map<ActivityDto>(activity));
        }

        public async Task<ResponseModel<ActivityDto>> UpdateActivityAsync(ActivityDto activity)
        {
            var _activity = await _unitOfWork.ActivityRepository.GetByIdAsync(activity.Id);
            if(_activity == null)
            {
                return ResponseModel<ActivityDto>.Failure(StatusCodes.Status404NotFound, $"Activity with id = {activity.Id} does not exist");
            }

            var checkCompleteness = await CheckDataCompleteness(activity);
            if(!checkCompleteness.Item1)
            {
                return ResponseModel<ActivityDto>.Failure(StatusCodes.Status404NotFound, checkCompleteness.Item2);
            }

            _activity.EmployeeId = activity.EmployeeId;
            _activity.ProjectId = activity.ProjectId;
            _activity.ActivityTypeId = activity.ActivityTypeId;
            _activity.RoleId = activity.RoleId;
            try
            {
                _unitOfWork.ActivityRepository.Update(_activity);
                await _unitOfWork.SaveAsync();
            }
            catch(Exception ex)
            {
                return ResponseModel<ActivityDto>.Failure(StatusCodes.Status500InternalServerError, $"Update process failed. Message: {ex.Message}");
            }
            return ResponseModel<ActivityDto>.Success(_mapper.Map<ActivityDto>(_activity));
        }

        private async Task<(bool, string)> CheckDataCompleteness(ActivityDto activity)
        {
            var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(activity.EmployeeId);
            if(employee == null)
                return (false, $"Employee with id = {activity.EmployeeId} does not exist");

            var project = await _unitOfWork.ProjectRepository.GetByIdAsync(activity.ProjectId);
            if(project == null)
                return (false, $"Project with id = {activity.ProjectId} does not exist");

            var activityType = await _unitOfWork.ActivityTypeRepository.GetByIdAsync(activity.ActivityTypeId);
            if(activityType == null)
                return (false, $"Activity type with id = {activity.ActivityTypeId} does not exist");

            var role = await _unitOfWork.RoleRepository.GetByIdAsync(activity.RoleId);
            if(role == null)
                return (false, $"Role with id = {activity.RoleId} does not exist");

            return (true, string.Empty);
        }

        public async Task<ResponseModel<string>> GetTimetrackingByDate(int employeeId, DateTime date)
        {
            var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(employeeId);
            if(employee == null)
            {
                return ResponseModel<string>.Failure(StatusCodes.Status400BadRequest, $"Employee with id = {employeeId} does not exist");
            }
            string report = await MakeReport(employee, date, date);
            return ResponseModel<string>.Success(report);
        }

        public async Task<ResponseModel<string>> GetTimetrackingByWeek(int employeeId, int year, int week)
        {
            if(year < 0 || year > DateTime.Now.Year)
            {
                return ResponseModel<string>.Failure(StatusCodes.Status400BadRequest, "Invalid year");
            }
            if(week < 1 || week > 52)
            {
                return ResponseModel<string>.Failure(StatusCodes.Status400BadRequest, "Week must be between 1 and 52");
            }
            var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(employeeId);
            if(employee == null)
            {
                return ResponseModel<string>.Failure(StatusCodes.Status400BadRequest, $"Employee with id = {employeeId} does not exist");
            }
            DateTime firstDayOfYear = new(year, 1, 1);
            DateTime firstDayOfWeek = firstDayOfYear.AddDays((week - 1) * 7);
            DateTime lastDayOfWeek = firstDayOfWeek.AddDays(6);
            string report = await MakeReport(employee, firstDayOfWeek, lastDayOfWeek);
            return ResponseModel<string>.Success(report);
        }

        private async Task<string> MakeReport(Employee employee, DateTime from, DateTime to)
        {
            StringBuilder report = new StringBuilder();
            var allProjects = await _unitOfWork.ProjectRepository.GetAllAsync();
            List<Project> filteredProjects;
            if(from == to)
            {
                filteredProjects = allProjects.Where(p => p.StartDate <= from && p.EndDate >= from).ToList();
            }
            else
            {
                filteredProjects = allProjects.Where(p => p.StartDate >= from && p.EndDate <= to).ToList();
            }
            if(filteredProjects.Count == 0)
            {
                return $"{employee.Name} did not work this date";
            }
            var activities = await _unitOfWork.ActivityRepository.GetAllAsync();
            foreach(var project in filteredProjects)
            {
                var activity = activities.FirstOrDefault(a => a.EmployeeId == employee.Id && a.ProjectId == project.Id);
                if(activity == null)
                    continue;

                var role = await _unitOfWork.RoleRepository.GetByIdAsync(activity.RoleId);
                var activityType = await _unitOfWork.ActivityTypeRepository.GetByIdAsync(activity.ActivityTypeId);
                report.AppendLine($"{employee.Name} worked on project {project.Name} {activityType.Name} as a {role.Name} from {project.StartDate} to {project.EndDate}");
            }
            return report.ToString();
        }
    }
}
