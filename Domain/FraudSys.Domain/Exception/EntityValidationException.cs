namespace FraudSys.Domain.Exception;

public class EntityValidationException : System.Exception
{
    public EntityValidationException(string message) : base(message)
    {

    }

    public EntityValidationException(string message, System.Exception innerException) : base(message, innerException)
    {

    }
}