namespace FraudSys.Domain.Exception;

public class TransactionException : System.Exception
{
    public TransactionException(string message) : base(message)
    {
    }
}