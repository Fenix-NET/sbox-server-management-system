using Microsoft.AspNetCore.Mvc;

namespace EconomyService.Api.Controllers
{
    [ApiVersion("1")]
    [ApiController]
    [Route("api/v1/economy")]
    public class EconomyController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<EconomyController> _logger;

        public EconomyController(ILogger<EconomyController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "balance/{id}")]
        public async Task<IActionResult> GetBalance(int id)
        {
            return Ok();
        }
        [HttpGet(Name = "transactionhistory/{id}")]
        public async Task<IActionResult> GetTransactionHistory(int id)
        {
            return Ok();
        }
    }
}
