using Microsoft.EntityFrameworkCore;
using MicroTaskAPI.Application.Interfaces.Repositories;
using MicroTaskAPI.Domain.Entities;
using MicroTaskAPI.Infrastructure.Persistence.Context;

namespace MicroTaskAPI.Infrastructure.Persistence.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly AppDbContext _context;

        public NotificationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Notification?> GetByIdAsync(int id) =>
            await _context.Notifications.FirstOrDefaultAsync(n => n.Id == id);

        public async Task<List<Notification>> GetByEmailAsync(string email) =>
            await _context.Notifications
                .Include(n => n.ToUser)
                .Where(n => n.ToUser.Email == email)
                .OrderByDescending(n => n.Time)
                .ToListAsync();

        public async Task<int> GetUnreadCountAsync(string email) =>
            await _context.Notifications
                .Include(n => n.ToUser)
                .CountAsync(n => n.ToUser.Email == email && !n.IsRead);

        public async Task<Notification> AddAsync(Notification notification)
        {
            await _context.Notifications.AddAsync(notification);
            await _context.SaveChangesAsync();
            return notification;
        }

        public async Task UpdateAsync(Notification notification)
        {
            _context.Notifications.Update(notification);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Notification notification)
        {
            _context.Notifications.Remove(notification);
            await _context.SaveChangesAsync();
        }
    }
}