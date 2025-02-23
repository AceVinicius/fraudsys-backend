namespace FraudSys.Infra.AWS.DynamoDB.Exception;

public sealed class DatabaseException : System.Exception
{
    public DatabaseException(string message, System.Exception innerException) : base(message, innerException)
    {

    }
}