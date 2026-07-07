using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MicroTaskAPI.Application.Interfaces.Services;

namespace MicroTaskAPI.API.Controllers
{
    [ApiController]
    [Route("api/stats")]
    [Authorize]
    public class StatsController : ControllerBase
    {
        private readonly IStatsService _statsService;

        public StatsController(IStatsService statsService)
        {
            _statsService = statsService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("admin")]
        public async Task<IActionResult> GetAdminStats()
        {
            var result = await _statsService.GetAdminStatsAsync();
            return Ok(result);
        }

        [Authorize(Roles = "Buyer")]
        [HttpGet("buyer/{email}")]
        public async Task<IActionResult> GetBuyerStats(string email)
        {
            var result = await _statsService.GetBuyerStatsAsync(email);
            return Ok(result);
        }

        [Authorize(Roles = "Worker")]
        [HttpGet("worker/{email}")]
        public async Task<IActionResult> GetWorkerStats(string email)
        {
            var result = await _statsService.GetWorkerStatsAsync(email);
            return Ok(result);
        }
    }
}