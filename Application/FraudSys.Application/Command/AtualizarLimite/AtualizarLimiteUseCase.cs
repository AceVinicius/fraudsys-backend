namespace FraudSys.Application.Command.AtualizarLimite;

public class AtualizarLimiteUseCase : ICommand<AtualizarLimiteInput, AtualizarLimiteOutput>
{
    private readonly ILogger<AtualizarLimiteUseCase> _logger;
    private readonly ILimiteClienteRepository _limiteClienteRepository;
    private readonly IAtualizarLimiteClienteValidator _validator;
    private readonly IUnitOfWork _unitOfWork;

    public AtualizarLimiteUseCase(
        ILogger<AtualizarLimiteUseCase> logger,
        ILimiteClienteRepository limiteClienteRepository,
        IAtualizarLimiteClienteValidator validator,
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _limiteClienteRepository = limiteClienteRepository;
        _validator = validator;
        _unitOfWork = unitOfWork;
    }

    public async Task<AtualizarLimiteOutput> Execute(
        AtualizarLimiteInput request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Atualizando limite para o cliente {Documento}.", request.Documento);

        var limiteCliente = await _limiteClienteRepository.GetByIdAsync(request.Documento, cancellationToken);

        if (limiteCliente == null)
        {
            _logger.LogError("LimiteCliente não encontrado.");

            return new AtualizarLimiteOutput(
                Success: false,
                Message: "LimiteCliente não encontrado."
            );
        }

        if (! _validator.Validate(request.NovoLimite))
        {
            _logger.LogError("Requisição inválida.");

            return new AtualizarLimiteOutput(
                Success: false,
                Message: "Limite não pode ser menor que 0."
            );
        }

        limiteCliente.AtualizarLimite(request.NovoLimite);

        await _limiteClienteRepository.UpdateAsync(limiteCliente, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        _logger.LogInformation("Limite atualizado com sucesso.");

        return new AtualizarLimiteOutput(
            Success: true,
            Message: "Limite atualizado com sucesso"
        );
    }
}