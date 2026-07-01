namespace MicroTaskAPI.Domain.Exceptions
{
    public class DuplicateEntityException : Exception
    {
        public DuplicateEntityException(string entityName, string field, object value)
            : base($"{entityName} with {field} '{value}' already exists.")
        {
        }

        public DuplicateEntityException(string message) : base(message)
        {
        }
    }
}