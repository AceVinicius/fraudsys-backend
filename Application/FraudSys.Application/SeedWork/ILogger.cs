namespace FraudSys.Application.SeedWork;

public interface ILogger
{
    void LogError(string message);
    void LogInformation(string message);
    void LogWarning(string message);
}
