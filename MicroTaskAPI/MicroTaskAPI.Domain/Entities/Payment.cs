using MicroTaskAPI.Domain.Common;
using MicroTaskAPI.Domain.Enums;

namespace MicroTaskAPI.Domain.Entities
{
    public class Payment : BaseEntity
    {
        public int BuyerId { get; set; }
        public User Buyer { get; set; } = null!;

        public int CoinPurchased { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } = string.Empty; 
        public string SenderNumber { get; set; } = string.Empty;
        public string TransactionId { get; set; } = string.Empty;
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
        public string? RejectionReason { get; set; }
    }
}