using Microsoft.AspNetCore.Mvc;

namespace NotificationService.Api.Controllers
{
    [ApiVersion("1")]
    [ApiController]
    [Route("api/v1/notification")]
    public class NotificationController : ControllerBase
    {
        private readonly ILogger<NotificationController> _logger;

        public NotificationController(ILogger<NotificationController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "get")]
        public async Task<IActionResult> Get()
        {
            return Ok();
        }
    }
}
