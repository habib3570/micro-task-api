using Microsoft.EntityFrameworkCore;
using MicroTaskAPI.Application.Interfaces.Repositories;
using MicroTaskAPI.Domain.Entities;
using MicroTaskAPI.Infrastructure.Persistence.Context;

namespace MicroTaskAPI.Infrastructure.Persistence.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly AppDbContext _context;

        public TaskRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<TaskItem?> GetByIdAsync(int id) =>
            await _context.Tasks.Include(t => t.Buyer).FirstOrDefaultAsync(t => t.Id == id);

        public async Task<List<TaskItem>> GetAvailableTasksAsync() =>
            await _context.Tasks
                .Include(t => t.Buyer)
                .Where(t => t.RequiredWorkers > 0)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();

        public async Task<List<TaskItem>> GetByBuyerEmailAsync(string buyerEmail) =>
            await _context.Tasks
                .Include(t => t.Buyer)
                .Where(t => t.Buyer.Email == buyerEmail)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();

        public async Task<List<TaskItem>> GetAllAsync() =>
            await _context.Tasks
                .Include(t => t.Buyer)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();

        public async Task<TaskItem> AddAsync(TaskItem task)
        {
            await _context.Tasks.AddAsync(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task UpdateAsync(TaskItem task)
        {
            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(TaskItem task)
        {
            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
        }

        public async Task<int> CountByBuyerAsync(int buyerId) =>
            await _context.Tasks.CountAsync(t => t.BuyerId == buyerId);

        public async Task<int> CountPendingByBuyerAsync(int buyerId) =>
            await _context.Tasks.CountAsync(t => t.BuyerId == buyerId && t.RequiredWorkers > 0);
    }
}