using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MicroTaskAPI.Application.DTOs.Submission;
using MicroTaskAPI.Application.Interfaces.Services;
using System.Security.Claims;

namespace MicroTaskAPI.API.Controllers
{
    [ApiController]
    [Route("api/submissions")]
    [Authorize]
    public class SubmissionsController : ControllerBase
    {
        private readonly ISubmissionService _submissionService;

        public SubmissionsController(ISubmissionService submissionService)
        {
            _submissionService = submissionService;
        }

        [Authorize(Roles = "Worker")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SubmissionCreateDto dto)
        {
            var email = User.FindFirstValue(ClaimTypes.Email)
                ?? throw new UnauthorizedAccessException("Email claim not found.");

            var result = await _submissionService.CreateAsync(email, dto);
            return Ok(result);
        }

        [Authorize(Roles = "Worker")]
        [HttpGet("worker/{email}")]
        public async Task<IActionResult> GetByWorker(string email, [FromQuery] int? page, [FromQuery] int? size)
        {
            if (page.HasValue && size.HasValue)
            {
                var paged = await _submissionService.GetByWorkerEmailPagedAsync(email, page.Value, size.Value);
                return Ok(paged);
            }

            var result = await _submissionService.GetByWorkerEmailAsync(email);
            return Ok(result);
        }

        [Authorize(Roles = "Buyer")]
        [HttpGet("buyer/{email}")]
        public async Task<IActionResult> GetPendingByBuyer(string email)
        {
            var result = await _submissionService.GetPendingByBuyerEmailAsync(email);
            return Ok(result);
        }

        [Authorize(Roles = "Buyer")]
        [HttpPut("{id}/approve")]
        public async Task<IActionResult> Approve(int id)
        {
            await _submissionService.ApproveAsync(id);
            return NoContent();
        }

        [Authorize(Roles = "Buyer")]
        [HttpPut("{id}/reject")]
        public async Task<IActionResult> Reject(int id)
        {
            await _submissionService.RejectAsync(id);
            return NoContent();
        }

        [Authorize(Roles = "Worker")]
        [HttpGet("worker/{email}/approved")]
        public async Task<IActionResult> GetApprovedByWorker(string email)
        {
            var result = await _submissionService.GetApprovedByWorkerEmailAsync(email);
            return Ok(result);
        }

        [Authorize(Roles = "Worker")]
        [HttpGet("worker/{email}/stats")]
        public async Task<IActionResult> GetWorkerStats(string email)
        {
            var result = await _submissionService.GetWorkerStatsAsync(email);
            return Ok(result);
        }
    }
}