namespace FraudSys.Application.Query.BuscarTodasTransacoes;

public class BuscarTodasTransacoesUseCase : IBuscarTodasTransacoesUseCase
{
    private readonly IAppLogger<BuscarTodasTransacoesUseCase> _appLogger;
    private readonly ITransacaoRepository _transacaoRepository;

    public BuscarTodasTransacoesUseCase(
        IAppLogger<BuscarTodasTransacoesUseCase> appLogger,
        ITransacaoRepository transacaoRepository)
    {
        _appLogger = appLogger;
        _transacaoRepository = transacaoRepository;
    }

    public async Task<BuscarTodasTransacoesOutput> Execute(
        BuscarTodasTransacoesInput input,
        CancellationToken cancellationToken)
    {
        _appLogger.LogInformation("Buscando todos limiteClientes");

        var transacoes = await _transacaoRepository.GetAllAsync(cancellationToken);

        _appLogger.LogInformation($"Foram encontrados {transacoes.Count()} limiteClientes.");

        return new BuscarTodasTransacoesOutput(transacoes);
    }
}