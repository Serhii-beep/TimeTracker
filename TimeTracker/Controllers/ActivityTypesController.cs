using Microsoft.AspNetCore.Mvc;
using TimeTracker.Services;
using TimeTracker.Services.Dtos;

namespace TimeTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivityTypesController : ControllerBase
    {
        private readonly ActivityTypeService _activityTypeService;

        public ActivityTypesController(ActivityTypeService activityTypeService)
        {
            _activityTypeService = activityTypeService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ActivityTypeDto>>> GetAll()
        {
            var response = await _activityTypeService.GetAllActivityTypesAsync();
            return response.IsSuccess ? Ok(response.Data) : StatusCode(response.StatusCode, response.Error);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ActivityTypeDto>> GetById(int id)
        {
            var response = await _activityTypeService.GetActivityTypeById(id);
            return response.IsSuccess ? Ok(response.Data) : StatusCode(response.StatusCode, response.Error);
        }

        [HttpPost]
        public async Task<ActionResult<ActivityTypeDto>> Create(ActivityTypeDto activityType)
        {
            var response = await _activityTypeService.CreateActivityTypeAsync(activityType);
            return response.IsSuccess ? CreatedAtAction("GetById", new { id = response.Data.Id }, response.Data) : StatusCode(response.StatusCode, response.Error);
        }

        [HttpPut]
        public async Task<ActionResult<ActivityTypeDto>> Update(ActivityTypeDto activityType)
        {
            var response = await _activityTypeService.UpdateActivityTypeAsync(activityType);
            return response.IsSuccess ? Ok(response.Data) : StatusCode(response.StatusCode, response.Error);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            var response = await _activityTypeService.DeleteActivityTypeByIdAsync(id);
            return response.IsSuccess ? Ok(response.Data) : StatusCode(response.StatusCode, response.Error);
        }
    }
}
