using Microsoft.EntityFrameworkCore;
using MicroTaskAPI.Application.Interfaces.Repositories;
using MicroTaskAPI.Domain.Entities;
using MicroTaskAPI.Domain.Enums;
using MicroTaskAPI.Infrastructure.Persistence.Context;

namespace MicroTaskAPI.Infrastructure.Persistence.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly AppDbContext _context;

        public PaymentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Payment?> GetByIdAsync(int id) =>
            await _context.Payments.Include(p => p.Buyer).FirstOrDefaultAsync(p => p.Id == id);

        public async Task<List<Payment>> GetByBuyerEmailAsync(string buyerEmail) =>
            await _context.Payments
                .Include(p => p.Buyer)
                .Where(p => p.Buyer.Email == buyerEmail)
                .OrderByDescending(p => p.PaymentDate)
                .ToListAsync();

        public async Task<List<Payment>> GetPendingAsync() =>
            await _context.Payments
                .Include(p => p.Buyer)
                .Where(p => p.Status == PaymentStatus.Pending)
                .OrderByDescending(p => p.PaymentDate)
                .ToListAsync();

        public async Task<Payment> AddAsync(Payment payment)
        {
            await _context.Payments.AddAsync(payment);
            await _context.SaveChangesAsync();
            return payment;
        }

        public async Task UpdateAsync(Payment payment)
        {
            _context.Payments.Update(payment);
            await _context.SaveChangesAsync();
        }

        public async Task<decimal> GetTotalPaymentsAsync() =>
            await _context.Payments
                .Where(p => p.Status == PaymentStatus.Success)
                .SumAsync(p => p.Amount);
    }
}