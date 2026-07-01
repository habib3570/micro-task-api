namespace MicroTaskAPI.Domain.Exceptions
{
    public class InsufficientCoinException : Exception
    {
        public InsufficientCoinException()
            : base("Insufficient coin balance to perform this action.")
        {
        }

        public InsufficientCoinException(int available, int required)
            : base($"Insufficient coin balance. Available: {available}, Required: {required}")
        {
        }

        public InsufficientCoinException(string message) : base(message)
        {
        }
    }
}