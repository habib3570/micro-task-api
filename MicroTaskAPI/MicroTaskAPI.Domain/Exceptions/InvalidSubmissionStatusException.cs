namespace MicroTaskAPI.Domain.Exceptions
{
    public class InvalidSubmissionStatusException : Exception
    {
        public InvalidSubmissionStatusException()
            : base("Submission status transition is not valid.")
        {
        }

        public InvalidSubmissionStatusException(string message) : base(message)
        {
        }
    }
}