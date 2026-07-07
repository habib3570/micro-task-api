using Microsoft.EntityFrameworkCore;
using MicroTaskAPI.Application.Interfaces.Repositories;
using MicroTaskAPI.Domain.Entities;
using MicroTaskAPI.Domain.Enums;
using MicroTaskAPI.Infrastructure.Persistence.Context;

namespace MicroTaskAPI.Infrastructure.Persistence.Repositories
{
    public class WithdrawalRepository : IWithdrawalRepository
    {
        private readonly AppDbContext _context;

        public WithdrawalRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Withdrawal?> GetByIdAsync(int id) =>
            await _context.Withdrawals.Include(w => w.Worker).FirstOrDefaultAsync(w => w.Id == id);

        public async Task<List<Withdrawal>> GetPendingAsync() =>
            await _context.Withdrawals
                .Include(w => w.Worker)
                .Where(w => w.Status == WithdrawalStatus.Pending)
                .OrderByDescending(w => w.WithdrawDate)
                .ToListAsync();

        public async Task<List<Withdrawal>> GetByWorkerEmailAsync(string workerEmail) =>
            await _context.Withdrawals
                .Include(w => w.Worker)
                .Where(w => w.Worker.Email == workerEmail)
                .OrderByDescending(w => w.WithdrawDate)
                .ToListAsync();

        public async Task<Withdrawal> AddAsync(Withdrawal withdrawal)
        {
            await _context.Withdrawals.AddAsync(withdrawal);
            await _context.SaveChangesAsync();
            return withdrawal;
        }

        public async Task UpdateAsync(Withdrawal withdrawal)
        {
            _context.Withdrawals.Update(withdrawal);
            await _context.SaveChangesAsync();
        }
    }
}