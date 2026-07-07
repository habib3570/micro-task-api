using MicroTaskAPI.Domain.Entities;

namespace MicroTaskAPI.Application.Interfaces.Repositories
{
    public interface IPaymentRepository
    {
        Task<Payment?> GetByIdAsync(int id);
        Task<List<Payment>> GetByBuyerEmailAsync(string buyerEmail);
        Task<Payment> AddAsync(Payment payment);
        Task UpdateAsync(Payment payment);
        Task<decimal> GetTotalPaymentsAsync();
    }
}