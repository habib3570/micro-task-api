namespace MicroTaskAPI.Application.DTOs.Withdrawal
{
    public class WithdrawalCreateDto
    {
        public int WithdrawalCoin { get; set; }
        public string PaymentSystem { get; set; } = string.Empty; 
        public string AccountNumber { get; set; } = string.Empty;
    }
}