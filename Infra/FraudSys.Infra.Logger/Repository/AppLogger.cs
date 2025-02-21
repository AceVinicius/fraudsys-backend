namespace FraudSys.Infra.Logger.Repository;

public class AppLogger<T> : IAppLogger<T>
{
    private readonly ILogger<T> _logger;

    public AppLogger(ILogger<T> logger)
    {
        _logger = logger;
    }

    public void LogTrace(string message)
    {
        _logger.LogTrace(BuildLogMessage("TRC", message));
    }

    public void LogDebug(string message)
    {
        _logger.LogDebug(BuildLogMessage("DBG", message));
    }

    public void LogInformation(string message)
    {
        _logger.LogInformation(BuildLogMessage("INF", message));
    }

    public void LogWarning(string message)
    {
        _logger.LogWarning(BuildLogMessage("WRN", message));
    }

    public void LogError(string message)
    {
        _logger.LogError(BuildLogMessage("ERR", message));
    }

    public void LogCritical(string message)
    {
        _logger.LogCritical(BuildLogMessage("CRT", message));
    }

    private static string BuildLogMessage(string level, string message)
    {
        return $"[{DateTime.Now} {level}] [{nameof(T)}] {message}";
    }
}