namespace FraudSys.Domain.Exception;

public class EntityCreationException : System.Exception
{
    public EntityCreationException(string message) : base(message)
    {

    }

    public EntityCreationException(string message, System.Exception innerException) : base(message, innerException)
    {

    }
}