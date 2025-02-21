using FraudSys.Application.Repository;

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
        _appLogger.LogInformation("Removendo limiteCliente");

        var limiteCliente = await _limiteClienteRepository.GetByIdAsync(
            input.Documento,
            cancellationToken);

        if (limiteCliente == null)
        {
            _appLogger.LogError("O limiteCliente não foi encontrado");

            return new RemoverLimiteOutput(
                false,
                "O limiteCliente não foi encontrado"
            );
        }

        _appLogger.LogInformation("O limiteCliente foi encontrado");

        await _limiteClienteRepository.DeleteAsync(limiteCliente.Documento, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        _appLogger.LogInformation("Limite removido com sucesso");

        return new RemoverLimiteOutput(
            true,
            "Limite removido com sucesso"
        );
    }
}