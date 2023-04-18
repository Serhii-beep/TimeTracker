using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TimeTracker.Services;

namespace TimeTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimeTrackingController : ControllerBase
    {
        private readonly ActivityService _activityService;

        public TimeTrackingController(ActivityService activityService)
        {
            _activityService = activityService;
        }

        [HttpGet("{employeeId:int}&{date:datetime}")]
        public async Task<ActionResult<string>> GetReportByDate(int employeeId, DateTime date)
        {
            var response = await _activityService.GetTimetrackingByDate(employeeId, date);
            return response.IsSuccess ? Ok(response.Data) : StatusCode(response.StatusCode, response.Error);
        }

        [HttpGet("{employeeId}&{week}&{year}")]
        public async Task<ActionResult<string>> GetReportByWeek(int employeeId, int week, int year)
        {
            var response = await _activityService.GetTimetrackingByWeek(employeeId, year, week);
            return response.IsSuccess ? Ok(response.Data) : StatusCode(response.StatusCode, response.Error);
        }
    }
}
