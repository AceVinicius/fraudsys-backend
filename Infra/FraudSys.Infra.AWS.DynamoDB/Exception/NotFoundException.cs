namespace FraudSys.Infra.AWS.DynamoDB.Exception;

public sealed class NotFoundException : System.Exception
{
    public NotFoundException(string message) : base(message)
    {

    }
}