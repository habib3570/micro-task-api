using MicroTaskAPI.Domain.Common;
using MicroTaskAPI.Domain.Enums;

namespace MicroTaskAPI.Domain.Entities
{
    public class Submission : BaseEntity
    {
        public int TaskId { get; set; }
        public TaskItem Task { get; set; } = null!;

        public string TaskTitle { get; set; } = string.Empty;
        public int PayableAmount { get; set; }

        public int WorkerId { get; set; }
        public User Worker { get; set; } = null!;

        public int BuyerId { get; set; }
        public User Buyer { get; set; } = null!;

        public string SubmissionDetail { get; set; } = string.Empty;
        public DateTime CurrentDate { get; set; } = DateTime.UtcNow;
        public SubmissionStatus Status { get; set; } = SubmissionStatus.Pending;
    }
}