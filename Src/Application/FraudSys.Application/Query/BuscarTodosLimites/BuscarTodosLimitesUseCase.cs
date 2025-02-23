namespace FraudSys.Application.Query.BuscarTodosLimites;

public class BuscarTodosLimitesUseCase : IBuscarTodosLimitesUseCase
{
    private readonly IAppLogger<BuscarTodosLimitesUseCase> _appLogger;
    private readonly ILimiteClienteRepository _limiteClienteRepository;

    public BuscarTodosLimitesUseCase(
        IAppLogger<BuscarTodosLimitesUseCase> appLogger,
        ILimiteClienteRepository limiteClienteRepository)
    {
        _appLogger = appLogger;
        _limiteClienteRepository = limiteClienteRepository;
    }

    public async Task<BuscarTodosLimitesOutput> Execute(BuscarTodosLimitesInput request,
        CancellationToken cancellationToken)
    {
        _appLogger.LogInformation("Buscando todos limiteClientes");

        var limitesClientes = await _limiteClienteRepository.GetAllAsync(
            cancellationToken);

        _appLogger.LogInformation($"Foram encontrados {limitesClientes.Count()} limiteClientes.");

        return new BuscarTodosLimitesOutput(limitesClientes);
    }
}