using FraudSys.Application.Command.EfetuarTransacao;
using FraudSys.Domain.Transacao.Validator;

namespace FraudSys.Service.API.Configuration;

public static class ApplicationConfiguration
{
    public static void AddApplicationConfigurations(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddInfraDependencies(configuration);
        services.AddApplicationDependencies();
        services.AddDomainDependencies();
    }

    private static void AddInfraDependencies(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.InjectInfraAwsDynamoDb(configuration);
        services.InjectLogger();
    }

    private static void AddApplicationDependencies(this IServiceCollection services)
    {
        // Use Cases
        services.AddScoped<ICadastrarLimiteUseCase, CadastrarLimiteUseCase>();
        services.AddScoped<IAtualizarLimiteUseCase, AtualizarLimiteUseCase>();
        services.AddScoped<IRemoverLimiteUseCase, RemoverLimiteUseCase>();
        services.AddScoped<IBuscarLimiteUseCase, BuscarLimiteUseCase>();
        services.AddScoped<IEfetuarTransacaoUseCase, EfetuarTransacaoUseCase>();
    }

    private static void AddDomainDependencies(this IServiceCollection services)
    {
        // Facades
        services.AddScoped<ILimiteClienteValidatorFacade, LimiteClienteValidatorFacade>();
        services.AddScoped<ITransacaoValidatorFacade, TransacaoValidatorFacade>();
    }
}