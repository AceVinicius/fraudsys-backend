namespace FraudSys.Application.Command.EfetuarTransacao;

public class EfetuarTransacaoUseCase : IEfetuarTransacaoUseCase
{
    private readonly IAppLogger<EfetuarTransacaoUseCase> _appLogger;
    private readonly ITransacaoValidatorFacade _transacaoValidator;
    private readonly ITransacaoRepository _transacaoRepository;
    private readonly ILimiteClienteRepository _limiteClienteRepository;
    private readonly IUnitOfWork _unitOfWork;

    public EfetuarTransacaoUseCase(
        IAppLogger<EfetuarTransacaoUseCase> appLogger,
        ITransacaoValidatorFacade transacaoValidator,
        ITransacaoRepository transacaoRepository,
        ILimiteClienteRepository limiteClienteRepository,
        IUnitOfWork unitOfWork)
    {
        _appLogger = appLogger;
        _transacaoValidator = transacaoValidator;
        _transacaoRepository = transacaoRepository;
        _limiteClienteRepository = limiteClienteRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<EfetuarTransacaoOutput> Execute(
        EfetuarTransacaoInput input,
        CancellationToken cancellationToken)
    {
        _appLogger.LogInformation("Iniciando use case para efetuar transação.");

        var pagador = await _limiteClienteRepository.GetByIdAsync(
            input.DocumentoPagador,
            cancellationToken);

        var recebedor = await _limiteClienteRepository.GetByIdAsync(
            input.DocumentoRecebedor,
            cancellationToken);

        var transacao = TransacaoEntity.Create(
            _transacaoValidator,
            pagador,
            recebedor,
            input.ValorTransacao);

        await _transacaoRepository.CreateAsync(transacao, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        try
        {
            transacao.EfetuarTransacao();
        }
        catch (TransactionException ex)
        {
            _appLogger.LogError(
                $"Transação rejeitada. Não será efetuada transferência de valores. Mensagem: {ex.Message}");
        }

        if (transacao.Status == StatusTransacao.Aprovada)
        {
            _appLogger.LogInformation("Transação aprovada. Efetuando transferência de valores.");

            await _limiteClienteRepository.TransferirAsync(
                pagador,
                recebedor,
                transacao,
                cancellationToken);
        }

        await _transacaoRepository.UpdateStatusAsync(transacao, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        _appLogger.LogInformation("Transação finalizada com sucesso.");

        return new EfetuarTransacaoOutput(transacao);
    }
}