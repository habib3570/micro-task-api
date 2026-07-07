namespace MicroTaskAPI.Application.DTOs.Task
{
    public class TaskUpdateDto
    {
        public string TaskTitle { get; set; } = string.Empty;
        public string TaskDetail { get; set; } = string.Empty;
        public string SubmissionInfo { get; set; } = string.Empty;
    }
}