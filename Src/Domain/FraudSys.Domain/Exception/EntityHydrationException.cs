namespace FraudSys.Domain.Exception;

public class EntityHydrationException(string message, System.Exception innerException)
    : System.Exception(message, innerException);