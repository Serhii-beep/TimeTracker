using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TimeTracker.Services.Dtos;
using TimeTracker.Services;

namespace TimeTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly ProjectService _projectService;

        public ProjectsController(ProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectDto>>> GetAll()
        {
            var response = await _projectService.GetAllProjectsAsync();
            return response.IsSuccess ? Ok(response.Data) : StatusCode(response.StatusCode, response.Error);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectDto>> GetById(int id)
        {
            var response = await _projectService.GetProjectById(id);
            return response.IsSuccess ? Ok(response.Data) : StatusCode(response.StatusCode, response.Error);
        }

        [HttpPost]
        public async Task<ActionResult<ProjectDto>> Create(ProjectDto project)
        {
            var response = await _projectService.CreateProjectAsync(project);
            return response.IsSuccess ? CreatedAtAction("GetById", new { id = response.Data.Id }, response.Data) : StatusCode(response.StatusCode, response.Error);
        }

        [HttpPut]
        public async Task<ActionResult<ProjectDto>> Update(ProjectDto project)
        {
            var response = await _projectService.UpdateProjectAsync(project);
            return response.IsSuccess ? Ok(response.Data) : StatusCode(response.StatusCode, response.Error);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            var response = await _projectService.DeleteProjectByIdAsync(id);
            return response.IsSuccess ? Ok(response.Data) : StatusCode(response.StatusCode, response.Error);
        }
    }
}
