namespace FraudSys.Infra.Logger.IoC;

public static class LoggerExtensions
{
    public static void InjectLogger(this IServiceCollection services)
    {
        services.AddSingleton(typeof(IAppLogger<>), typeof(AppLogger<>));
    }
}