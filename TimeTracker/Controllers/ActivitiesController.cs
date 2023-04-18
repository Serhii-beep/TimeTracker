using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TimeTracker.Services.Dtos;
using TimeTracker.Services;

namespace TimeTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivitiesController : ControllerBase
    {
        private readonly ActivityService _activityService;

        public ActivitiesController(ActivityService activityService)
        {
            _activityService = activityService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ActivityDto>>> GetAll()
        {
            var response = await _activityService.GetAllActivitiesAsync();
            return response.IsSuccess ? Ok(response.Data) : StatusCode(response.StatusCode, response.Error);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ActivityDto>> GetById(int id)
        {
            var response = await _activityService.GetActivityById(id);
            return response.IsSuccess ? Ok(response.Data) : StatusCode(response.StatusCode, response.Error);
        }

        [HttpPost]
        public async Task<ActionResult<ActivityDto>> Create(ActivityDto activity)
        {
            var response = await _activityService.CreateActivityAsync(activity);
            return response.IsSuccess ? CreatedAtAction("GetById", new { id = response.Data.Id }, response.Data) : StatusCode(response.StatusCode, response.Error);
        }

        [HttpPut]
        public async Task<ActionResult<ActivityDto>> Update(ActivityDto activity)
        {
            var response = await _activityService.UpdateActivityAsync(activity);
            return response.IsSuccess ? Ok(response.Data) : StatusCode(response.StatusCode, response.Error);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            var response = await _activityService.DeleteActivityByIdAsync(id);
            return response.IsSuccess ? Ok(response.Data) : StatusCode(response.StatusCode, response.Error);
        }
    }
}
