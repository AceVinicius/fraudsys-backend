namespace FraudSys.Service.API.Configuration;

public static class ControllerConfiguration
{
    public static void AddControllerConfigurations(this IServiceCollection services)
    {
        services.AddControllers(
            options =>
            {
                options.Filters.Add<ApiGlobalExceptionFilter>();
            }
        );
        // services.AddScoped<AppLogger>();
    }
}