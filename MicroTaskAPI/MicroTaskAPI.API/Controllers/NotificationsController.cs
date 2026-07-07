using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MicroTaskAPI.Application.Interfaces.Services;

namespace MicroTaskAPI.API.Controllers
{
    [ApiController]
    [Route("api/notifications")]
    [Authorize]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationsController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet("{email}")]
        public async Task<IActionResult> GetByEmail(string email)
        {
            var result = await _notificationService.GetByEmailAsync(email);
            return Ok(result);
        }

        [HttpPut("{id}/read")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            await _notificationService.MarkAsReadAsync(id);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _notificationService.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("{email}/unread-count")]
        public async Task<IActionResult> GetUnreadCount(string email)
        {
            var result = await _notificationService.GetUnreadCountAsync(email);
            return Ok(new { unreadCount = result });
        }
    }
}