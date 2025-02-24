namespace FraudSys.Application.Query.BuscarLimite;

public class BuscarLimiteUseCase : IBuscarLimiteUseCase
{
    private readonly IAppLogger<BuscarLimiteUseCase> _appLogger;
    private readonly ILimiteClienteRepository _limiteClienteRepository;

    public BuscarLimiteUseCase(
        IAppLogger<BuscarLimiteUseCase> appLogger,
        ILimiteClienteRepository limiteClienteRepository)
    {
        _appLogger = appLogger;
        _limiteClienteRepository = limiteClienteRepository;
    }

    public async Task<BuscarLimiteOutput> Execute(BuscarLimiteInput input, CancellationToken
            cancellationToken)
    {
        _appLogger.LogInformation($"Iniciando use case para buscar LimiteCliente '{input.Documento}'.");

        var limiteCliente = await _limiteClienteRepository.GetByIdAsync(
            input.Documento,
            cancellationToken);

        _appLogger.LogInformation("LimiteCliente encontrado com sucesso.");

        return new BuscarLimiteOutput(limiteCliente);
    }
}