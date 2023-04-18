using AutoMapper;
using TimeTracker.Data.Entities;
using TimeTracker.Data.Interfaces;
using TimeTracker.Services.Dtos;

namespace TimeTracker.Services
{
    public class RoleService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        public RoleService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseModel<RoleDto>> CreateRoleAsync(RoleDto role)
        {
            var allRoles = await _unitOfWork.RoleRepository.GetAllAsync();
            if(allRoles.Any(r => r.Name.ToLower() == role.Name.ToLower()))
            {
                return ResponseModel<RoleDto>.Failure(StatusCodes.Status400BadRequest, $"Role '{role.Name}' already exists");
            }
            var roleEntity = _mapper.Map<Role>(role);
            try
            {
                await _unitOfWork.RoleRepository.AddAsync(roleEntity);
                await _unitOfWork.SaveAsync();
            }
            catch(Exception ex)
            {
                return ResponseModel<RoleDto>.Failure(StatusCodes.Status500InternalServerError, $"Creation process failed. Message: {ex.Message}");
            }
            return ResponseModel<RoleDto>.Success(_mapper.Map<RoleDto>(roleEntity));
        }

        public async Task<ResponseModel<bool>> DeleteRoleByIdAsync(int id)
        {
            var role = await _unitOfWork.RoleRepository.GetByIdAsync(id);
            if(role == null)
            {
                return ResponseModel<bool>.Failure(StatusCodes.Status404NotFound, $"Role with id = {id} does not exist");
            }
            try
            {
                await _unitOfWork.RoleRepository.DeleteByIdAsync(id);
                await _unitOfWork.SaveAsync();
            }
            catch(Exception ex)
            {
                return ResponseModel<bool>.Failure(StatusCodes.Status500InternalServerError, $"Deletion process failed. Message: {ex.Message}");
            }
            return ResponseModel<bool>.Success(true);
        }

        public async Task<ResponseModel<IEnumerable<RoleDto>>> GetAllRolesAsync()
        {
            var roles = await _unitOfWork.RoleRepository.GetAllAsync();
            return ResponseModel<IEnumerable<RoleDto>>.Success(_mapper.Map<IEnumerable<RoleDto>>(roles));
        }

        public async Task<ResponseModel<RoleDto>> GetRoleById(int id)
        {
            var role = await _unitOfWork.RoleRepository.GetByIdAsync(id);
            if(role == null)
            {
                return ResponseModel<RoleDto>.Failure(StatusCodes.Status404NotFound, $"Role with id = {id} does not exist");
            }
            return ResponseModel<RoleDto>.Success(_mapper.Map<RoleDto>(role));
        }

        public async Task<ResponseModel<RoleDto>> UpdateRoleAsync(RoleDto role)
        {
            var _role = await _unitOfWork.RoleRepository.GetByIdAsync(role.Id);
            if(_role == null)
            {
                return ResponseModel<RoleDto>.Failure(StatusCodes.Status404NotFound, $"Role with id = {role.Id} does not exist");
            }
            _role.Name = role.Name;
            try
            {
                _unitOfWork.RoleRepository.Update(_role);
                await _unitOfWork.SaveAsync();
            }
            catch(Exception ex)
            {
                return ResponseModel<RoleDto>.Failure(StatusCodes.Status500InternalServerError, $"Update process failed. Message: {ex.Message}");
            }
            return ResponseModel<RoleDto>.Success(_mapper.Map<RoleDto>(_role));
        }
    }
}
