using AutoMapper;
using TimeTracker.Data.Entities;
using TimeTracker.Data.Interfaces;
using TimeTracker.Services.Dtos;

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
    }
}
