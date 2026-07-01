namespace MicroTaskAPI.Domain.Exceptions
{
    public class InvalidWithdrawalException : Exception
    {
        public InvalidWithdrawalException()
            : base("Withdrawal request does not meet the minimum required coin amount.")
        {
        }

        public InvalidWithdrawalException(string message) : base(message)
        {
        }
    }
}