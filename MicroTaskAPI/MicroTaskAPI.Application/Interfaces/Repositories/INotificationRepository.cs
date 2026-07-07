using MicroTaskAPI.Domain.Entities;

namespace MicroTaskAPI.Application.Interfaces.Repositories
{
    public interface INotificationRepository
    {
        Task<Notification?> GetByIdAsync(int id);
        Task<List<Notification>> GetByEmailAsync(string email);
        Task<int> GetUnreadCountAsync(string email);
        Task<Notification> AddAsync(Notification notification);
        Task UpdateAsync(Notification notification);
        Task DeleteAsync(Notification notification);
    }
}