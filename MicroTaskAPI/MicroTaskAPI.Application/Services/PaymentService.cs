using MicroTaskAPI.Application.DTOs.Payment;
using MicroTaskAPI.Application.Interfaces.Repositories;
using MicroTaskAPI.Application.Interfaces.Services;
using MicroTaskAPI.Domain.Entities;
using MicroTaskAPI.Domain.Enums;
using MicroTaskAPI.Domain.Exceptions;

namespace MicroTaskAPI.Application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IUserRepository _userRepository;
        private readonly INotificationService _notificationService;

        public PaymentService(
            IPaymentRepository paymentRepository,
            IUserRepository userRepository,
            INotificationService notificationService)
        {
            _paymentRepository = paymentRepository;
            _userRepository = userRepository;
            _notificationService = notificationService;
        }

        public async Task<PaymentResponseDto> CreateAsync(string buyerEmail, PaymentCreateDto dto)
        {
            var buyer = await _userRepository.GetByEmailAsync(buyerEmail)
                ?? throw new EntityNotFoundException("User", buyerEmail);

            var payment = new Payment
            {
                BuyerId = buyer.Id,
                CoinPurchased = dto.CoinPurchased,
                Amount = dto.Amount,
                PaymentMethod = dto.PaymentMethod,
                SenderNumber = dto.SenderNumber,
                TransactionId = dto.TransactionId,
                Status = PaymentStatus.Pending
            };

            await _paymentRepository.AddAsync(payment);

            payment.Buyer = buyer;
            return MapToDto(payment);
        }

        public async Task<List<PaymentResponseDto>> GetByBuyerEmailAsync(string buyerEmail)
        {
            var payments = await _paymentRepository.GetByBuyerEmailAsync(buyerEmail);
            return payments.Select(MapToDto).ToList();
        }

        public async Task<List<PaymentResponseDto>> GetPendingAsync()
        {
            var payments = await _paymentRepository.GetPendingAsync();
            return payments.Select(MapToDto).ToList();
        }

        public async Task ApproveAsync(int id)
        {
            var payment = await _paymentRepository.GetByIdAsync(id)
                ?? throw new EntityNotFoundException("Payment", id);

            if (payment.Status != PaymentStatus.Pending)
                throw new InvalidSubmissionStatusException("Only pending payments can be approved.");

            payment.Status = PaymentStatus.Success;
            await _paymentRepository.UpdateAsync(payment);

            var buyer = await _userRepository.GetByIdAsync(payment.BuyerId);
            if (buyer != null)
            {
                buyer.Coin += payment.CoinPurchased;
                await _userRepository.UpdateAsync(buyer);
            }

            await _notificationService.CreateAsync(
                payment.BuyerId,
                $"Your payment of {payment.Amount} was approved. {payment.CoinPurchased} coins added.",
                "/dashboard/buyer-home");
        }

        public async Task RejectAsync(int id, PaymentRejectDto dto)
        {
            var payment = await _paymentRepository.GetByIdAsync(id)
                ?? throw new EntityNotFoundException("Payment", id);

            if (payment.Status != PaymentStatus.Pending)
                throw new InvalidSubmissionStatusException("Only pending payments can be rejected.");

            payment.Status = PaymentStatus.Failed;
            payment.RejectionReason = dto.RejectionReason;
            await _paymentRepository.UpdateAsync(payment);

            await _notificationService.CreateAsync(
                payment.BuyerId,
                $"Your payment of {payment.Amount} was rejected. Reason: {dto.RejectionReason}",
                "/dashboard/buyer-home");
        }

        public async Task<decimal> GetTotalPaymentsAsync() =>
            await _paymentRepository.GetTotalPaymentsAsync();

        private static PaymentResponseDto MapToDto(Payment payment) => new()
        {
            Id = payment.Id,
            BuyerEmail = payment.Buyer?.Email ?? string.Empty,
            CoinPurchased = payment.CoinPurchased,
            Amount = payment.Amount,
            PaymentMethod = payment.PaymentMethod,
            SenderNumber = payment.SenderNumber,
            TransactionId = payment.TransactionId,
            PaymentDate = payment.PaymentDate,
            Status = payment.Status.ToString(),
            RejectionReason = payment.RejectionReason
        };
    }
}