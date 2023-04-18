using AutoMapper;
using TimeTracker.Data.Entities;
using TimeTracker.Data.Interfaces;
using TimeTracker.Services.Dtos;

namespace TimeTracker.Services
{
    public class EmployeeService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        public EmployeeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseModel<EmployeeDto>> CreateEmployeeAsync(EmployeeDto employee)
        {
            var allEmployees = await _unitOfWork.EmployeeRepository.GetAllAsync();
            if(allEmployees.Any(e => e.Name.ToLower() == employee.Name.ToLower()))
            {
                return ResponseModel<EmployeeDto>.Failure(StatusCodes.Status400BadRequest, $"Employee '{employee.Name}' already exists");
            }
            
            if(!IsValidAge(employee.BirthDate))
            {
                return ResponseModel<EmployeeDto>.Failure(StatusCodes.Status400BadRequest, $"The age of employee must be between 18 and 100 years");
            }
            var employeeEntity = _mapper.Map<Employee>(employee);
            try
            {
                await _unitOfWork.EmployeeRepository.AddAsync(employeeEntity);
                await _unitOfWork.SaveAsync();
            }
            catch(Exception ex)
            {
                return ResponseModel<EmployeeDto>.Failure(StatusCodes.Status500InternalServerError, $"Creation process failed. Message: {ex.Message}");
            }
            return ResponseModel<EmployeeDto>.Success(_mapper.Map<EmployeeDto>(employeeEntity));
        }

        public async Task<ResponseModel<bool>> DeleteEmployeeByIdAsync(int id)
        {
            var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(id);
            if(employee == null)
            {
                return ResponseModel<bool>.Failure(StatusCodes.Status404NotFound, $"Employee with id = {id} does not exist");
            }
            try
            {
                await _unitOfWork.EmployeeRepository.DeleteByIdAsync(id);
                await _unitOfWork.SaveAsync();
            }
            catch(Exception ex)
            {
                return ResponseModel<bool>.Failure(StatusCodes.Status500InternalServerError, $"Deletion process failed. Message: {ex.Message}");
            }
            return ResponseModel<bool>.Success(true);
        }

        public async Task<ResponseModel<IEnumerable<EmployeeDto>>> GetAllEmployeesAsync()
        {
            var employees = await _unitOfWork.EmployeeRepository.GetAllAsync();
            return ResponseModel<IEnumerable<EmployeeDto>>.Success(_mapper.Map<IEnumerable<EmployeeDto>>(employees));
        }

        public async Task<ResponseModel<EmployeeDto>> GetEmployeeById(int id)
        {
            var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(id);
            if(employee == null)
            {
                return ResponseModel<EmployeeDto>.Failure(StatusCodes.Status404NotFound, $"Employee with id = {id} does not exist");
            }
            return ResponseModel<EmployeeDto>.Success(_mapper.Map<EmployeeDto>(employee));
        }

        public async Task<ResponseModel<EmployeeDto>> UpdateEmployeeAsync(EmployeeDto employee)
        {
            var _employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(employee.Id);
            if(_employee == null)
            {
                return ResponseModel<EmployeeDto>.Failure(StatusCodes.Status404NotFound, $"Employee with id = {employee.Id} does not exist");
            }
            if(!IsValidAge(employee.BirthDate))
            {
                return ResponseModel<EmployeeDto>.Failure(StatusCodes.Status400BadRequest, $"The age of employee must be between 18 and 100 years");
            }
            _employee.Name = employee.Name;
            _employee.Sex = employee.Sex;
            _employee.BirthDate = employee.BirthDate;
            try
            {
                _unitOfWork.EmployeeRepository.Update(_employee);
                await _unitOfWork.SaveAsync();
            }
            catch(Exception ex)
            {
                return ResponseModel<EmployeeDto>.Failure(StatusCodes.Status500InternalServerError, $"Update process failed. Message: {ex.Message}");
            }
            return ResponseModel<EmployeeDto>.Success(_mapper.Map<EmployeeDto>(_employee));
        }

        private bool IsValidAge(DateTime birthDate)
        {
            int age = DateTime.Now.Year - birthDate.Year;
            if(DateTime.Now.Month < birthDate.Month || (DateTime.Now.Month == birthDate.Month
                && DateTime.Now.Day < birthDate.Day))
            {
                --age;
            }
            return age >= 18 && age <= 100;
        }
    }
}
