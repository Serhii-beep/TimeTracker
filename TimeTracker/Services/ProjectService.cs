using AutoMapper;
using TimeTracker.Data.Entities;
using TimeTracker.Data.Interfaces;
using TimeTracker.Services.Dtos;

namespace TimeTracker.Services
{
    public class ProjectService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        public ProjectService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseModel<ProjectDto>> CreateProjectAsync(ProjectDto project)
        {
            if(project.StartDate > project.EndDate)
            {
                return ResponseModel<ProjectDto>.Failure(StatusCodes.Status400BadRequest, "Project's start date must be before the end date");
            }
            var projectEntity = _mapper.Map<Project>(project);
            try
            {
                await _unitOfWork.ProjectRepository.AddAsync(projectEntity);
                await _unitOfWork.SaveAsync();
            }
            catch(Exception ex)
            {
                return ResponseModel<ProjectDto>.Failure(StatusCodes.Status500InternalServerError, $"Creation process failed. Message: {ex.Message}");
            }
            return ResponseModel<ProjectDto>.Success(_mapper.Map<ProjectDto>(projectEntity));
        }

        public async Task<ResponseModel<bool>> DeleteProjectByIdAsync(int id)
        {
            var project = await _unitOfWork.ProjectRepository.GetByIdAsync(id);
            if(project == null)
            {
                return ResponseModel<bool>.Failure(StatusCodes.Status404NotFound, $"Project with id = {id} does not exist");
            }
            try
            {
                await _unitOfWork.ProjectRepository.DeleteByIdAsync(id);
                await _unitOfWork.SaveAsync();
            }
            catch(Exception ex)
            {
                return ResponseModel<bool>.Failure(StatusCodes.Status500InternalServerError, $"Deletion process failed. Message: {ex.Message}");
            }
            return ResponseModel<bool>.Success(true);
        }

        public async Task<ResponseModel<IEnumerable<ProjectDto>>> GetAllProjectsAsync()
        {
            var projects = await _unitOfWork.ProjectRepository.GetAllAsync();
            return ResponseModel<IEnumerable<ProjectDto>>.Success(_mapper.Map<IEnumerable<ProjectDto>>(projects));
        }

        public async Task<ResponseModel<ProjectDto>> GetProjectById(int id)
        {
            var project = await _unitOfWork.ProjectRepository.GetByIdAsync(id);
            if(project == null)
            {
                return ResponseModel<ProjectDto>.Failure(StatusCodes.Status404NotFound, $"Project with id = {id} does not exist");
            }
            return ResponseModel<ProjectDto>.Success(_mapper.Map<ProjectDto>(project));
        }

        public async Task<ResponseModel<ProjectDto>> UpdateProjectAsync(ProjectDto project)
        {
            var _project = await _unitOfWork.ProjectRepository.GetByIdAsync(project.Id);
            if(_project == null)
            {
                return ResponseModel<ProjectDto>.Failure(StatusCodes.Status404NotFound, $"Project with id = {project.Id} does not exist");
            }
            if(project.StartDate > project.EndDate)
            {
                return ResponseModel<ProjectDto>.Failure(StatusCodes.Status400BadRequest, "Project's start date must be before the end date");
            }
            _project.Name = project.Name;
            _project.StartDate = project.StartDate;
            _project.EndDate = project.EndDate;
            try
            {
                _unitOfWork.ProjectRepository.Update(_project);
                await _unitOfWork.SaveAsync();
            }
            catch(Exception ex)
            {
                return ResponseModel<ProjectDto>.Failure(StatusCodes.Status500InternalServerError, $"Update process failed. Message: {ex.Message}");
            }
            return ResponseModel<ProjectDto>.Success(_mapper.Map<ProjectDto>(_project));
        }
    }
}
