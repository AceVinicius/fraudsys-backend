namespace FraudSys.Infra.Logger.UnitTests.Repository;

public class AppLoggerTest
{
    private readonly Mock<ILogger<AppLoggerTest>> _mockLogger;
    private readonly AppLogger<AppLoggerTest> _appLogger;

    public AppLoggerTest()
    {
        _mockLogger = new Mock<ILogger<AppLoggerTest>>();
        _appLogger = new AppLogger<AppLoggerTest>(_mockLogger.Object);
    }

    [Fact]
    public void LogTrace_ShouldLogTraceMessage()
    {
        // Arrange
        var message = "Trace message";

        // Act
        _appLogger.LogTrace(message);

        // Assert
        _mockLogger.Verify(
            logger => logger.Log(
                LogLevel.Trace,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains(message) == true),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public void LogDebug_ShouldLogDebugMessage()
    {
        // Arrange
        var message = "Debug message";

        // Act
        _appLogger.LogDebug(message);

        // Assert
        _mockLogger.Verify(
            logger => logger.Log(
                LogLevel.Debug,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains(message)),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public void LogInformation_ShouldLogInformationMessage()
    {
        // Arrange
        var message = "Information message";

        // Act
        _appLogger.LogInformation(message);

        // Assert
        _mockLogger.Verify(
            logger => logger.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains(message)),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public void LogWarning_ShouldLogWarningMessage()
    {
        // Arrange
        var message = "Warning message";

        // Act
        _appLogger.LogWarning(message);

        // Assert
        _mockLogger.Verify(
            logger => logger.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains(message)),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public void LogError_ShouldLogErrorMessage()
    {
        // Arrange
        var message = "Error message";

        // Act
        _appLogger.LogError(message);

        // Assert
        _mockLogger.Verify(
            logger => logger.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains(message)),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public void LogCritical_ShouldLogCriticalMessage()
    {
        // Arrange
        var message = "Critical message";

        // Act
        _appLogger.LogCritical(message);

        // Assert
        _mockLogger.Verify(
            logger => logger.Log(
                LogLevel.Critical,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains(message)),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }
}