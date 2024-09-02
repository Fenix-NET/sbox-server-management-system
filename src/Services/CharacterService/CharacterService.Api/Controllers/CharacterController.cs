using Microsoft.AspNetCore.Mvc;

namespace CharacterService.Api.Controllers
{
    [ApiVersion("1")]
    [ApiController]
    [Route("api/v1/character")]
    public class CharacterController : ControllerBase
    {
        private readonly ILogger<CharacterController> _logger;

        public CharacterController(ILogger<CharacterController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "getinfo/{id}")]
        public async Task<IActionResult> GetCharacterInfo(int id)
        {
            return Ok();
        }

    }
}
