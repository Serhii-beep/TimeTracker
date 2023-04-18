using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TimeTracker.Services.Dtos;
using TimeTracker.Services;

namespace TimeTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly RoleService _roleService;

        public RolesController(RoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoleDto>>> GetAll()
        {
            var response = await _roleService.GetAllRolesAsync();
            return response.IsSuccess ? Ok(response.Data) : StatusCode(response.StatusCode, response.Error);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RoleDto>> GetById(int id)
        {
            var response = await _roleService.GetRoleById(id);
            return response.IsSuccess ? Ok(response.Data) : StatusCode(response.StatusCode, response.Error);
        }

        [HttpPost]
        public async Task<ActionResult<RoleDto>> Create(RoleDto role)
        {
            var response = await _roleService.CreateRoleAsync(role);
            return response.IsSuccess ? CreatedAtAction("GetById", new { id = response.Data.Id }, response.Data) : StatusCode(response.StatusCode, response.Error);
        }

        [HttpPut]
        public async Task<ActionResult<RoleDto>> Update(RoleDto role)
        {
            var response = await _roleService.UpdateRoleAsync(role);
            return response.IsSuccess ? Ok(response.Data) : StatusCode(response.StatusCode, response.Error);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            var response = await _roleService.DeleteRoleByIdAsync(id);
            return response.IsSuccess ? Ok(response.Data) : StatusCode(response.StatusCode, response.Error);
        }
    }
}
