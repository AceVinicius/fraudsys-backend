namespace FraudSys.Application.Command.RemoverLimite;

public class RemoverLimiteUseCase : IRemoverLimiteUseCase
{
    private readonly IAppLogger<RemoverLimiteUseCase> _appLogger;
    private readonly ILimiteClienteRepository _limiteClienteRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RemoverLimiteUseCase(
        IAppLogger<RemoverLimiteUseCase> appLogger,
        ILimiteClienteRepository limiteClienteRepository,
        IUnitOfWork unitOfWork)
    {
        _appLogger = appLogger;
        _limiteClienteRepository = limiteClienteRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<RemoverLimiteOutput> Execute(
        RemoverLimiteInput input,
        CancellationToken cancellationToken)
    {
        _appLogger.LogInformation($"Iniciando use case de remoção do LimiteCliente '{input.Documento}'.");

        var limiteCliente = await _limiteClienteRepository.GetByIdAsync(
            input.Documento,
            cancellationToken);

        await _limiteClienteRepository.DeleteAsync(limiteCliente.Documento, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        _appLogger.LogInformation("LimiteCliente remoção com sucesso.");

        return new RemoverLimiteOutput(limiteCliente);
    }
}