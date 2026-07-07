using MicroTaskAPI.Domain.Entities;
using MicroTaskAPI.Domain.Enums;

namespace MicroTaskAPI.Application.Interfaces.Repositories
{
    public interface IWithdrawalRepository
    {
        Task<Withdrawal?> GetByIdAsync(int id);
        Task<List<Withdrawal>> GetPendingAsync();
        Task<List<Withdrawal>> GetByWorkerEmailAsync(string workerEmail);
        Task<Withdrawal> AddAsync(Withdrawal withdrawal);
        Task UpdateAsync(Withdrawal withdrawal);
    }
}