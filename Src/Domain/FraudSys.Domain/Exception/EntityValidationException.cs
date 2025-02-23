namespace FraudSys.Domain.Exception;

public class EntityValidationException(string message) : System.Exception(message);