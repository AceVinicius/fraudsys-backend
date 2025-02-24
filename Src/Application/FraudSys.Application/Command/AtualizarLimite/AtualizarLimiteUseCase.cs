namespace FraudSys.Application.Command.AtualizarLimite;

public class AtualizarLimiteUseCase : IAtualizarLimiteUseCase
{
    private readonly IAppLogger<AtualizarLimiteUseCase> _appLogger;
    private readonly ILimiteClienteRepository _limiteClienteRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AtualizarLimiteUseCase(
        IAppLogger<AtualizarLimiteUseCase> appLogger,
        ILimiteClienteRepository limiteClienteRepository,
        IUnitOfWork unitOfWork)
    {
        _appLogger = appLogger;
        _limiteClienteRepository = limiteClienteRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<AtualizarLimiteOutput> Execute(
        AtualizarLimiteInput input,
        CancellationToken cancellationToken)
    {
        _appLogger.LogInformation($"Iniciando use case para atualização do LimiteCliente '{input.Documento}'.");

        var limiteCliente = await _limiteClienteRepository.GetByIdAsync(
            input.Documento,
            cancellationToken);

        limiteCliente.AtualizarLimite(input.NovoLimite);

        await _limiteClienteRepository.UpdateAsync(limiteCliente, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        _appLogger.LogInformation("LimiteCliente atualizado com sucesso.");

        return new AtualizarLimiteOutput(limiteCliente);
    }
}