namespace MicroTaskAPI.Application.DTOs.Notification
{
    public class NotificationResponseDto
    {
        public int Id { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? ActionRoute { get; set; }
        public DateTime Time { get; set; }
        public bool IsRead { get; set; }
    }
}