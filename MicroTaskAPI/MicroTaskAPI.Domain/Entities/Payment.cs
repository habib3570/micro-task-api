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
        public string PaymentMethod { get; set; } = "Stripe";
        public string? TransactionId { get; set; }
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
    }
}