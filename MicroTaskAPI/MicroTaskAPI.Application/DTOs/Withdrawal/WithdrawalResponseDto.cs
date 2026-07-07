namespace MicroTaskAPI.Application.DTOs.Withdrawal
{
    public class WithdrawalResponseDto
    {
        public int Id { get; set; }
        public string WorkerEmail { get; set; } = string.Empty;
        public string WorkerName { get; set; } = string.Empty;
        public int WithdrawalCoin { get; set; }
        public decimal WithdrawalAmount { get; set; }
        public string PaymentSystem { get; set; } = string.Empty;
        public string AccountNumber { get; set; } = string.Empty;
        public DateTime WithdrawDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? RejectionReason { get; set; }
    }
}