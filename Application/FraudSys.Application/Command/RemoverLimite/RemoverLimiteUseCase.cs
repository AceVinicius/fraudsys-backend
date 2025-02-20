namespace FraudSys.Application.Command.RemoverLimite;

public class RemoverLimiteUseCase : ICommand<RemoverLimiteInput, RemoverLimiteOutput>
{
    private readonly Logger<RemoverLimiteUseCase> _logger;
    private readonly ILimiteClienteRepository _limiteClienteRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RemoverLimiteUseCase(
        Logger<RemoverLimiteUseCase> logger,
        ILimiteClienteRepository limiteClienteRepository,
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _limiteClienteRepository = limiteClienteRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<RemoverLimiteOutput> Execute(
        RemoverLimiteInput input,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Removendo limiteCliente");

        var limiteCliente = await _limiteClienteRepository.GetByIdAsync(
            input.Documento,
            cancellationToken);

        if (limiteCliente == null)
        {
            _logger.LogError("O limiteCliente não foi encontrado");

            return new RemoverLimiteOutput(
                false,
                "O limiteCliente não foi encontrado"
            );
        }

        _logger.LogInformation("O limiteCliente foi encontrado");

        await _limiteClienteRepository.DeleteAsync(limiteCliente.Documento, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        _logger.LogInformation("Limite removido com sucesso");

        return new RemoverLimiteOutput(
            true,
            "Limite removido com sucesso"
        );
    }
}