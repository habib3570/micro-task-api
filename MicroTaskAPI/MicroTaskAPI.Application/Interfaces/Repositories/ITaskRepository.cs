using MicroTaskAPI.Domain.Entities;

namespace MicroTaskAPI.Application.Interfaces.Repositories
{
    public interface ITaskRepository
    {
        Task<TaskItem?> GetByIdAsync(int id);
        Task<List<TaskItem>> GetAvailableTasksAsync();
        Task<List<TaskItem>> GetByBuyerEmailAsync(string buyerEmail);
        Task<List<TaskItem>> GetAllAsync();
        Task<TaskItem> AddAsync(TaskItem task);
        Task UpdateAsync(TaskItem task);
        Task DeleteAsync(TaskItem task);
        Task<int> CountByBuyerAsync(int buyerId);
        Task<int> CountPendingByBuyerAsync(int buyerId);
    }
}