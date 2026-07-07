using MicroTaskAPI.Application.DTOs.Payment;

namespace MicroTaskAPI.Application.Interfaces.Services
{
    public interface IPaymentService
    {
        Task<PaymentResponseDto> CreateAsync(string buyerEmail, PaymentCreateDto dto);
        Task<List<PaymentResponseDto>> GetByBuyerEmailAsync(string buyerEmail);
        Task<List<PaymentResponseDto>> GetPendingAsync();
        Task ApproveAsync(int id);
        Task RejectAsync(int id, PaymentRejectDto dto);
        Task<decimal> GetTotalPaymentsAsync();
    }
}