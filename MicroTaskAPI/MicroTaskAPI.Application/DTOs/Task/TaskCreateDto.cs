namespace MicroTaskAPI.Application.DTOs.Task
{
    public class TaskCreateDto
    {
        public string TaskTitle { get; set; } = string.Empty;
        public string TaskDetail { get; set; } = string.Empty;
        public int RequiredWorkers { get; set; }
        public int PayableAmount { get; set; }
        public DateTime CompletionDate { get; set; }
        public string SubmissionInfo { get; set; } = string.Empty;
        public string? TaskImageUrl { get; set; }
    }
}