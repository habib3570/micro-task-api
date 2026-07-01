using MicroTaskAPI.Domain.Common;

namespace MicroTaskAPI.Domain.Entities
{
    public class Notification : BaseEntity
    {
        public string Message { get; set; } = string.Empty;

        public int ToUserId { get; set; }
        public User ToUser { get; set; } = null!;

        public string? ActionRoute { get; set; }
        public DateTime Time { get; set; } = DateTime.UtcNow;
        public bool IsRead { get; set; } = false;
    }
}