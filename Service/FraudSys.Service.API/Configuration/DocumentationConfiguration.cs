namespace FraudSys.Service.API.Configuration;

public static class DocumentationConfiguration
{
    public static void AddDocumentation(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }

    public static void UseDocumentation(this WebApplication app)
    {
        // if (!app.Environment.IsDevelopment()) return;

        app.UseSwagger();
        app.UseSwaggerUI();
    }
}