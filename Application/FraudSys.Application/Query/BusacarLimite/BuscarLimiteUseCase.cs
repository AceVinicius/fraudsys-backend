namespace FraudSys.Application.Query.BusacarLimite;

public class BuscarLimiteUseCase : IQuery<BuscarLimiteInput, BuscarLimiteOutput>
{
    private readonly ILogger<BuscarLimiteUseCase> _logger;
    private readonly ILimiteClienteRepository _limiteClienteRepository;

    public BuscarLimiteUseCase(
        ILogger<BuscarLimiteUseCase> logger,
        ILimiteClienteRepository limiteClienteRepository)
    {
        _logger = logger;
        _limiteClienteRepository = limiteClienteRepository;
    }

    public async Task<BuscarLimiteOutput> Execute(BuscarLimiteInput request, CancellationToken
            cancellationToken)
    {
        _logger.LogInformation("Buscando limiteCliente");

        var limiteCliente = await _limiteClienteRepository.GetByIdAsync(
            request.Documento,
            cancellationToken);

        if (limiteCliente == null)
        {
            _logger.LogError("O limiteCliente não foi encontrado");

            return new BuscarLimiteOutput(
                "",
                "",
                "",
                0
            );
        }

        _logger.LogInformation("O limiteCliente foi encontrado");

        return new BuscarLimiteOutput(
            limiteCliente.Documento,
            limiteCliente.NumeroAgencia,
            limiteCliente.NumeroConta,
            limiteCliente.LimiteTransacao
        );
    }
}