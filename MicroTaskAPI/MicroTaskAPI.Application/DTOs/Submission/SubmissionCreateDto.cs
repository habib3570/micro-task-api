namespace MicroTaskAPI.Application.DTOs.Submission
{
    public class SubmissionCreateDto
    {
        public int TaskId { get; set; }
        public string SubmissionDetail { get; set; } = string.Empty;
    }
}