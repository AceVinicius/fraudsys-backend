namespace FraudSys.Infra.Logger.Repository;

public class AppLogger<T> : IAppLogger<T>
{
    private readonly string _className;
    private readonly ILogger<T> _logger;

    public AppLogger(ILogger<T> logger)
    {
        _logger = logger;
        _className = typeof(T).Name;
    }

    public void LogTrace(string message)
    {
        _logger.LogTrace(BuildLogMessage("TRC", _className, message));
    }

    public void LogDebug(string message)
    {
        _logger.LogDebug(BuildLogMessage("DBG", _className, message));
    }

    public void LogInformation(string message)
    {
        _logger.LogInformation(BuildLogMessage("INF", _className, message));
    }

    public void LogWarning(string message)
    {
        _logger.LogWarning(BuildLogMessage("WRN", _className, message));
    }

    public void LogError(string message)
    {
        _logger.LogError(BuildLogMessage("ERR", _className, message));
    }

    public void LogCritical(string message)
    {
        _logger.LogCritical(BuildLogMessage("CRT", _className, message));
    }

    private static string BuildLogMessage(string level, string filename, string message)
    {
        return $"[{DateTime.Now} {level}] {filename} - {message}";
    }
}