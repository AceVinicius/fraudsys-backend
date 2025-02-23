namespace FraudSys.Application.Command.EfetuarTransacao;

public class EfetuarTransacaoUseCase : IEfetuarTransacaoUseCase
{
    private readonly IAppLogger<EfetuarTransacaoUseCase> _appLogger;
    private readonly ITransacaoRepository _transacaoRepository;
    private readonly ILimiteClienteRepository _limiteClienteRepository;
    private readonly IUnitOfWork _unitOfWork;

    public EfetuarTransacaoUseCase(
        IAppLogger<EfetuarTransacaoUseCase> appLogger,
        ITransacaoRepository transacaoRepository,
        ILimiteClienteRepository limiteClienteRepository,
        IUnitOfWork unitOfWork
        )
    {
        _appLogger = appLogger;
        _transacaoRepository = transacaoRepository;
        _limiteClienteRepository = limiteClienteRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<EfetuarTransacaoOutput> Execute(
        EfetuarTransacaoInput input,
        CancellationToken cancellationToken)
    {
        _appLogger.LogInformation("Efetuando transação");

        var pagador = await _limiteClienteRepository.GetByIdAsync(
            input.DocumentoPagador,
            cancellationToken);

        var recebedor = await _limiteClienteRepository.GetByIdAsync(
            input.DocumentoRecebedor,
            cancellationToken);

        var transacao = new TransacaoEntity(
            pagador,
            recebedor,
            input.ValorTransacao);

        await _transacaoRepository.CreateAsync(transacao, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        transacao.EfetuarTransacao();

        if (transacao.Status == StatusTransacao.Aprovada)
        {
            await _limiteClienteRepository.TransferirAsync(
                pagador,
                recebedor,
                transacao,
                cancellationToken);
        }

        await _transacaoRepository.UpdateStatusAsync(transacao, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return new EfetuarTransacaoOutput(transacao);
    }
}