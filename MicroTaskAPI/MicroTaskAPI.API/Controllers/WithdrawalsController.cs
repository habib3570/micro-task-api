using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MicroTaskAPI.Application.DTOs.Withdrawal;
using MicroTaskAPI.Application.Interfaces.Services;
using System.Security.Claims;

namespace MicroTaskAPI.API.Controllers
{
    [ApiController]
    [Route("api/withdrawals")]
    [Authorize]
    public class WithdrawalsController : ControllerBase
    {
        private readonly IWithdrawalService _withdrawalService;

        public WithdrawalsController(IWithdrawalService withdrawalService)
        {
            _withdrawalService = withdrawalService;
        }

        [Authorize(Roles = "Worker")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] WithdrawalCreateDto dto)
        {
            var email = User.FindFirstValue(ClaimTypes.Email)
                ?? throw new UnauthorizedAccessException("Email claim not found.");

            var result = await _withdrawalService.CreateAsync(email, dto);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("pending")]
        public async Task<IActionResult> GetPending()
        {
            var result = await _withdrawalService.GetPendingAsync();
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}/approve")]
        public async Task<IActionResult> Approve(int id)
        {
            await _withdrawalService.ApproveAsync(id);
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}/reject")]
        public async Task<IActionResult> Reject(int id, [FromBody] WithdrawalRejectDto dto)
        {
            await _withdrawalService.RejectAsync(id, dto);
            return NoContent();
        }

        [Authorize(Roles = "Worker")]
        [HttpGet("worker/{email}")]
        public async Task<IActionResult> GetByWorker(string email)
        {
            var result = await _withdrawalService.GetByWorkerEmailAsync(email);
            return Ok(result);
        }
    }
}