using Microsoft.AspNetCore.Mvc;

namespace AdminService.Api.Controllers
{
    [ApiVersion("1")]
    [ApiController]
    [Route("api/v1/admin")]
    public class AdminController : ControllerBase
    {
        private readonly ILogger<AdminController> _logger;

        public AdminController(ILogger<AdminController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "getalladmins")]
        public async Task<IActionResult> GetAllAdmins()
        {
            _logger.LogInformation("GetAllAdmins");
            return Ok();
        }

        [HttpGet(Name = "getplayerinfo/{id}")]
        public async Task<IActionResult> GetPlayerInfo(int id)
        {
            _logger.LogInformation($"GetPlayerInfo-id:{id}");
            return Ok();
        }

        [HttpGet(Name = "getallbannedplayers")]
        public async Task<IActionResult> GetAllBannedPlayers()
        {
            _logger.LogInformation("GetAllBannedPlayers");
            return Ok();
        }
    }
}
