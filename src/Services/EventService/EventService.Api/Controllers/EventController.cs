using Microsoft.AspNetCore.Mvc;

namespace EventService.Api.Controllers
{
    [ApiVersion("1")]
    [ApiController]
    [Route("api/v1/event")]
    public class EventController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<EventController> _logger;

        public EventController(ILogger<EventController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "getallevent")]
        public async Task<IActionResult> GetAllEvent()
        {
            _logger.LogInformation("GetAllEvent");
            return Ok();
        }

        [HttpPost(Name = "regonevent")]
        public async Task<IActionResult> RegistrationOnEvent(int id)
        {
            _logger.LogInformation("RegistrationOnEvent");
            return Ok();
        }
    }
}
