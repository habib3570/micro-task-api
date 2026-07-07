namespace MicroTaskAPI.Application.DTOs.Payment
{
    
    public class PaymentCreateDto
    {
        public int CoinPurchased { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } = string.Empty; 
        public string SenderNumber { get; set; } = string.Empty;  
        public string TransactionId { get; set; } = string.Empty; 
    }
}