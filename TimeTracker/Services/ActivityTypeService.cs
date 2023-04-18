using AutoMapper;
using TimeTracker.Data.Entities;
using TimeTracker.Data.Interfaces;
using TimeTracker.Services.Dtos;

namespace TimeTracker.Services
{
    public class ActivityTypeService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        public ActivityTypeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseModel<ActivityTypeDto>> CreateActivityTypeAsync(ActivityTypeDto activityType)
        {
            var allActivityTypes = await _unitOfWork.ActivityTypeRepository.GetAllAsync();
            if(allActivityTypes.Any(at => at.Name.ToLower() == activityType.Name.ToLower()))
            {
                return ResponseModel<ActivityTypeDto>.Failure(StatusCodes.Status400BadRequest, $"Activity type '{activityType.Name}' already exists");
            }
            var activityTypeEntity = _mapper.Map<ActivityType>(activityType);
            try
            {
                await _unitOfWork.ActivityTypeRepository.AddAsync(activityTypeEntity);
                await _unitOfWork.SaveAsync();
            }
            catch(Exception ex)
            {
                return ResponseModel<ActivityTypeDto>.Failure(StatusCodes.Status500InternalServerError, $"Creation process failed. Message: {ex.Message}");
            }
            return ResponseModel<ActivityTypeDto>.Success(_mapper.Map<ActivityTypeDto>(activityTypeEntity));
        }

        public async Task<ResponseModel<bool>> DeleteActivityTypeByIdAsync(int id)
        {
            var activityType = await _unitOfWork.ActivityTypeRepository.GetByIdAsync(id);
            if(activityType == null)
            {
                return ResponseModel<bool>.Failure(StatusCodes.Status404NotFound, $"Activity type with id = {id} does not exist");
            }
            try
            {
                await _unitOfWork.ActivityTypeRepository.DeleteByIdAsync(id);
                await _unitOfWork.SaveAsync();
            }
            catch(Exception ex)
            {
                return ResponseModel<bool>.Failure(StatusCodes.Status500InternalServerError, $"Deletion process failed. Message: {ex.Message}");
            }
            return ResponseModel<bool>.Success(true);
        }

        public async Task<ResponseModel<IEnumerable<ActivityTypeDto>>> GetAllActivityTypesAsync()
        {
            var activityTypes = await _unitOfWork.ActivityTypeRepository.GetAllAsync();
            return ResponseModel<IEnumerable<ActivityTypeDto>>.Success(_mapper.Map<IEnumerable<ActivityTypeDto>>(activityTypes));
        }

        public async Task<ResponseModel<ActivityTypeDto>> GetActivityTypeById(int id)
        {
            var activityType = await _unitOfWork.ActivityTypeRepository.GetByIdAsync(id);
            if(activityType == null)
            {
                return ResponseModel<ActivityTypeDto>.Failure(StatusCodes.Status404NotFound, $"Activity type with id = {id} does not exist");
            }
            return ResponseModel<ActivityTypeDto>.Success(_mapper.Map<ActivityTypeDto>(activityType));
        }

        public async Task<ResponseModel<ActivityTypeDto>> UpdateActivityTypeAsync(ActivityTypeDto activityType)
        {
            var _activityType = await _unitOfWork.ActivityTypeRepository.GetByIdAsync(activityType.Id);
            if( _activityType == null)
            {
                return ResponseModel<ActivityTypeDto>.Failure(StatusCodes.Status404NotFound, $"Activity type with id = {activityType.Id} does not exist");
            }
            _activityType.Name = activityType.Name;
            try
            {
                _unitOfWork.ActivityTypeRepository.Update(_activityType);
                await _unitOfWork.SaveAsync();
            }
            catch(Exception ex)
            {
                return ResponseModel<ActivityTypeDto>.Failure(StatusCodes.Status500InternalServerError, $"Update process failed. Message: {ex.Message}");
            }
            return ResponseModel<ActivityTypeDto>.Success(_mapper.Map<ActivityTypeDto>(_activityType));
        }
    }
}
