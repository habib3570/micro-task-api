using MicroTaskAPI.Application.DTOs.Notification;
using MicroTaskAPI.Application.Interfaces.Repositories;
using MicroTaskAPI.Application.Interfaces.Services;
using MicroTaskAPI.Domain.Entities;
using MicroTaskAPI.Domain.Exceptions;

namespace MicroTaskAPI.Application.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationService(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<List<NotificationResponseDto>> GetByEmailAsync(string email)
        {
            var notifications = await _notificationRepository.GetByEmailAsync(email);
            return notifications.Select(MapToDto).ToList();
        }

        public async Task MarkAsReadAsync(int id)
        {
            var notification = await _notificationRepository.GetByIdAsync(id)
                ?? throw new EntityNotFoundException("Notification", id);

            notification.IsRead = true;
            await _notificationRepository.UpdateAsync(notification);
        }

        public async Task DeleteAsync(int id)
        {
            var notification = await _notificationRepository.GetByIdAsync(id)
                ?? throw new EntityNotFoundException("Notification", id);

            await _notificationRepository.DeleteAsync(notification);
        }

        public async Task<int> GetUnreadCountAsync(string email) =>
            await _notificationRepository.GetUnreadCountAsync(email);

        public async Task CreateAsync(int toUserId, string message, string? actionRoute = null)
        {
            var notification = new Notification
            {
                ToUserId = toUserId,
                Message = message,
                ActionRoute = actionRoute,
                IsRead = false
            };

            await _notificationRepository.AddAsync(notification);
        }

        private static NotificationResponseDto MapToDto(Notification notification) => new()
        {
            Id = notification.Id,
            Message = notification.Message,
            ActionRoute = notification.ActionRoute,
            Time = notification.Time,
            IsRead = notification.IsRead
        };
    }
}