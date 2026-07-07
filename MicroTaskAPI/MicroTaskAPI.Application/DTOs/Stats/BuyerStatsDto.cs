namespace MicroTaskAPI.Application.DTOs.Stats
{
    public class BuyerStatsDto
    {
        public int TaskCount { get; set; }
        public int PendingTasks { get; set; }
        public decimal TotalPaid { get; set; }
    }
}