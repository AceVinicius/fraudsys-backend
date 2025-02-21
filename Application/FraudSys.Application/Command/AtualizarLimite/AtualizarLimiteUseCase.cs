using FraudSys.Application.Repository;

namespace FraudSys.Application.Command.AtualizarLimite;

public class AtualizarLimiteUseCase : IAtualizarLimiteUseCase
{
    private readonly IAppLogger<AtualizarLimiteUseCase> _appLogger;
    private readonly ILimiteClienteRepository _limiteClienteRepository;
    private readonly IAtualizarLimiteClienteValidator _validator;
    private readonly IUnitOfWork _unitOfWork;

    public AtualizarLimiteUseCase(
        IAppLogger<AtualizarLimiteUseCase> appLogger,
        ILimiteClienteRepository limiteClienteRepository,
        IAtualizarLimiteClienteValidator validator,
        IUnitOfWork unitOfWork)
    {
        _appLogger = appLogger;
        _limiteClienteRepository = limiteClienteRepository;
        _validator = validator;
        _unitOfWork = unitOfWork;
    }

    public async Task<AtualizarLimiteOutput> Execute(
        AtualizarLimiteInput input,
        CancellationToken cancellationToken)
    {
        _appLogger.LogInformation($"Atualizando limite para o cliente {input.Documento}.");

        var limiteCliente = await _limiteClienteRepository.GetByIdAsync(input.Documento, cancellationToken);

        if (limiteCliente == null)
        {
            _appLogger.LogError("LimiteCliente não encontrado.");

            return new AtualizarLimiteOutput(
                Success: false,
                Message: "LimiteCliente não encontrado."
            );
        }

        if (! _validator.Validate(input.NovoLimite))
        {
            _appLogger.LogError("Requisição inválida.");

            return new AtualizarLimiteOutput(
                Success: false,
                Message: "Limite não pode ser menor que 0."
            );
        }

        limiteCliente.AtualizarLimite(input.NovoLimite);

        await _limiteClienteRepository.UpdateAsync(limiteCliente, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        _appLogger.LogInformation("Limite atualizado com sucesso.");

        return new AtualizarLimiteOutput(
            Success: true,
            Message: "Limite atualizado com sucesso"
        );
    }
}