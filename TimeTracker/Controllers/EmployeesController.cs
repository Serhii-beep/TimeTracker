using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TimeTracker.Services.Dtos;
using TimeTracker.Services;

namespace TimeTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly EmployeeService _employeeService;

        public EmployeesController(EmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetAll()
        {
            var response = await _employeeService.GetAllEmployeesAsync();
            return response.IsSuccess ? Ok(response.Data) : StatusCode(response.StatusCode, response.Error);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeDto>> GetById(int id)
        {
            var response = await _employeeService.GetEmployeeById(id);
            return response.IsSuccess ? Ok(response.Data) : StatusCode(response.StatusCode, response.Error);
        }

        [HttpPost]
        public async Task<ActionResult<EmployeeDto>> Create(EmployeeDto employee)
        {
            var response = await _employeeService.CreateEmployeeAsync(employee);
            return response.IsSuccess ? CreatedAtAction("GetById", new { id = response.Data.Id }, response.Data) : StatusCode(response.StatusCode, response.Error);
        }

        [HttpPut]
        public async Task<ActionResult<EmployeeDto>> Update(EmployeeDto employee)
        {
            var response = await _employeeService.UpdateEmployeeAsync(employee);
            return response.IsSuccess ? Ok(response.Data) : StatusCode(response.StatusCode, response.Error);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            var response = await _employeeService.DeleteEmployeeByIdAsync(id);
            return response.IsSuccess ? Ok(response.Data) : StatusCode(response.StatusCode, response.Error);
        }
    }
}
