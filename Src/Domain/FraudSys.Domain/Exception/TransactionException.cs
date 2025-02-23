namespace FraudSys.Domain.Exception;

public class TransactionException(string message, System.Exception innerException)
    : System.Exception(message, innerException);