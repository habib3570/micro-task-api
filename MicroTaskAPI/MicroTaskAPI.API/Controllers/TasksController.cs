using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MicroTaskAPI.Application.DTOs.Task;
using MicroTaskAPI.Application.Interfaces.Services;
using System.Security.Claims;

namespace MicroTaskAPI.API.Controllers
{
    [ApiController]
    [Route("api/tasks")]
    [Authorize]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [Authorize(Roles = "Worker")]
        [HttpGet]
        public async Task<IActionResult> GetAvailable()
        {
            var result = await _taskService.GetAvailableTasksAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _taskService.GetByIdAsync(id);
            return Ok(result);
        }

        [Authorize(Roles = "Buyer")]
        [HttpGet("buyer/{email}")]
        public async Task<IActionResult> GetByBuyer(string email)
        {
            var result = await _taskService.GetByBuyerEmailAsync(email);
            return Ok(result);
        }

        [Authorize(Roles = "Buyer")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TaskCreateDto dto)
        {
            var email = User.FindFirstValue(ClaimTypes.Email)
                ?? throw new UnauthorizedAccessException("Email claim not found.");

            var result = await _taskService.CreateAsync(email, dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [Authorize(Roles = "Buyer")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] TaskUpdateDto dto)
        {
            var result = await _taskService.UpdateAsync(id, dto);
            return Ok(result);
        }

        [Authorize(Roles = "Buyer,Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _taskService.DeleteAsync(id);
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllForAdmin()
        {
            var result = await _taskService.GetAllForAdminAsync();
            return Ok(result);
        }
    }
}