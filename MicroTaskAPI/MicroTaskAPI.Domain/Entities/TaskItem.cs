using MicroTaskAPI.Domain.Common;

namespace MicroTaskAPI.Domain.Entities
{
    public class TaskItem : BaseEntity
    {
        public string TaskTitle { get; set; } = string.Empty;
        public string TaskDetail { get; set; } = string.Empty;
        public int RequiredWorkers { get; set; }
        public int PayableAmount { get; set; }
        public DateTime CompletionDate { get; set; }
        public string SubmissionInfo { get; set; } = string.Empty;
        public string? TaskImageUrl { get; set; }

        public int BuyerId { get; set; }
        public User Buyer { get; set; } = null!;

        // Navigation
        public ICollection<Submission> Submissions { get; set; } = new List<Submission>();
    }
}