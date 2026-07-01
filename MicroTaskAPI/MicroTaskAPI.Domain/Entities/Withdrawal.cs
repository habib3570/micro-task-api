using MicroTaskAPI.Domain.Common;
using MicroTaskAPI.Domain.Enums;

namespace MicroTaskAPI.Domain.Entities
{
    public class Withdrawal : BaseEntity
    {
        public int WorkerId { get; set; }
        public User Worker { get; set; } = null!;

        public string WorkerName { get; set; } = string.Empty;
        public int WithdrawalCoin { get; set; }
        public decimal WithdrawalAmount { get; set; }
        public string PaymentSystem { get; set; } = string.Empty; 
        public string AccountNumber { get; set; } = string.Empty;
        public DateTime WithdrawDate { get; set; } = DateTime.UtcNow;
        public WithdrawalStatus Status { get; set; } = WithdrawalStatus.Pending;
        public enum PaymentSystemType { Stripe, Bkash, Rocket, Nagad }
    }
}