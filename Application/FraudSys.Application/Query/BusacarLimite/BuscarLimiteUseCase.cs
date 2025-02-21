using FraudSys.Application.Repository;

namespace FraudSys.Application.Query.BusacarLimite;

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

    public async Task<BuscarLimiteOutput> Execute(BuscarLimiteInput request, CancellationToken
            cancellationToken)
    {
        _appLogger.LogInformation("Buscando limiteCliente");

        var limiteCliente = await _limiteClienteRepository.GetByIdAsync(
            request.Documento,
            cancellationToken);

        if (limiteCliente == null)
        {
            _appLogger.LogError("O limiteCliente não foi encontrado");

            return new BuscarLimiteOutput(
                "",
                "",
                "",
                0
            );
        }

        _appLogger.LogInformation("O limiteCliente foi encontrado");

        return new BuscarLimiteOutput(
            limiteCliente.Documento,
            limiteCliente.NumeroAgencia,
            limiteCliente.NumeroConta,
            limiteCliente.LimiteTransacao
        );
    }
}