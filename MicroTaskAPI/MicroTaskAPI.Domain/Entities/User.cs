using MicroTaskAPI.Domain.Common;
using MicroTaskAPI.Domain.Enums;

namespace MicroTaskAPI.Domain.Entities
{
    public class User : BaseEntity
    {
        public string DisplayName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhotoUrl { get; set; }
        public UserRole Role { get; set; }
        public int Coin { get; set; }
        public string? PasswordHash { get; set; }
        public string? FirebaseUid { get; set; }

        // Navigation Properties
        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
        public ICollection<Submission> WorkerSubmissions { get; set; } = new List<Submission>();
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
        public ICollection<Withdrawal> Withdrawals { get; set; } = new List<Withdrawal>();
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    }
}