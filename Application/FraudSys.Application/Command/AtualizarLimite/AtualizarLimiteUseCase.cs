using FraudSys.Application.Repository;

namespace FraudSys.Application.Command.AtualizarLimite;

public class AtualizarLimiteUseCase : IAtualizarLimiteUseCase
{
    private readonly IAppLogger<AtualizarLimiteUseCase> _appLogger;
    private readonly ILimiteClienteRepository _limiteClienteRepository;
    private readonly ILimiteClienteValidatorFacade _validatorFacade;
    private readonly IUnitOfWork _unitOfWork;

    public AtualizarLimiteUseCase(
        IAppLogger<AtualizarLimiteUseCase> appLogger,
        ILimiteClienteRepository limiteClienteRepository,
        ILimiteClienteValidatorFacade validatorFacade,
        IUnitOfWork unitOfWork)
    {
        _appLogger = appLogger;
        _limiteClienteRepository = limiteClienteRepository;
        _validatorFacade = validatorFacade;
        _unitOfWork = unitOfWork;
    }

    public async Task<AtualizarLimiteOutput> Execute(
        AtualizarLimiteInput input,
        CancellationToken cancellationToken)
    {
        _appLogger.LogInformation($"Atualizando limite para o cliente {input.Documento}.");

        var limiteCliente = await _limiteClienteRepository.GetByIdAsync(
            input.Documento,
            cancellationToken);

        _validatorFacade.ValidateAtualizacaoLimiteCliente(input.NovoLimite);

        limiteCliente.AtualizarLimite(input.NovoLimite);

        await _limiteClienteRepository.UpdateAsync(limiteCliente, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        _appLogger.LogInformation("Limite atualizado com sucesso.");

        return new AtualizarLimiteOutput(limiteCliente);
    }
}