namespace MicroTaskAPI.Domain.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(string entityName, object key)
            : base($"{entityName} with identifier '{key}' was not found.")
        {
        }

        public EntityNotFoundException(string message) : base(message)
        {
        }
    }
}