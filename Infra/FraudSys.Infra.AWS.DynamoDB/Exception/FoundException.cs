namespace FraudSys.Infra.AWS.DynamoDB.Exception;

public class FoundException : System.Exception
{
    public FoundException(string message) : base(message)
    {

    }
}