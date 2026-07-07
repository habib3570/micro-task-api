namespace MicroTaskAPI.Application.DTOs.Submission
{
    public class SubmissionResponseDto
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public string TaskTitle { get; set; } = string.Empty;
        public int PayableAmount { get; set; }
        public string WorkerEmail { get; set; } = string.Empty;
        public string WorkerName { get; set; } = string.Empty;
        public string BuyerEmail { get; set; } = string.Empty;
        public string BuyerName { get; set; } = string.Empty;
        public string SubmissionDetail { get; set; } = string.Empty;
        public DateTime CurrentDate { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}