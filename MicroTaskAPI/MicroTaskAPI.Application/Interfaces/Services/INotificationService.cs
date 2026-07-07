using MicroTaskAPI.Application.DTOs.Notification;

namespace MicroTaskAPI.Application.Interfaces.Services
{
    public interface INotificationService
    {
        Task<List<NotificationResponseDto>> GetByEmailAsync(string email);
        Task MarkAsReadAsync(int id);
        Task DeleteAsync(int id);
        Task<int> GetUnreadCountAsync(string email);
        Task CreateAsync(int toUserId, string message, string? actionRoute = null);
    }
}