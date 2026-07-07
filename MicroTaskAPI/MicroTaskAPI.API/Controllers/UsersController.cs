using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MicroTaskAPI.Application.DTOs.User;
using MicroTaskAPI.Application.Interfaces.Services;

namespace MicroTaskAPI.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _userService.GetAllUsersAsync();
            return Ok(result);
        }

        [HttpGet("{email}")]
        public async Task<IActionResult> GetByEmail(string email)
        {
            var result = await _userService.GetByEmailAsync(email);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{email}/role")]
        public async Task<IActionResult> UpdateRole(string email, [FromBody] UpdateRoleDto dto)
        {
            await _userService.UpdateRoleAsync(email, dto);
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{email}")]
        public async Task<IActionResult> Delete(string email)
        {
            await _userService.DeleteAsync(email);
            return NoContent();
        }

        [HttpGet("{email}/coin")]
        public async Task<IActionResult> GetCoin(string email)
        {
            var result = await _userService.GetCoinAsync(email);
            return Ok(new { coin = result });
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{email}/coin")]
        public async Task<IActionResult> UpdateCoin(string email, [FromBody] UpdateCoinDto dto)
        {
            await _userService.UpdateCoinAsync(email, dto);
            return NoContent();
        }

        [AllowAnonymous]
        [HttpGet("top-workers")]
        public async Task<IActionResult> GetTopWorkers()
        {
            var result = await _userService.GetTopWorkersAsync();
            return Ok(result);
        }
    }
}