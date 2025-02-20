namespace FraudSys.Application.Command.CadastrarLimite;

public class CadastrarLimiteUseCase : ICommand<CadastrarLimiteInput, CadastrarLimiteOutput>
{
    private readonly ILogger<CadastrarLimiteUseCase> _logger;
    private readonly ILimiteClienteRepository _limiteClienteRepository;
    private readonly ILimiteClienteValidator _limiteClienteValidator;
    private readonly IUnitOfWork _unitOfWork;

    public CadastrarLimiteUseCase(
        ILogger<CadastrarLimiteUseCase> logger,
        ILimiteClienteRepository limiteClienteRepository,
        ILimiteClienteValidator limiteClienteValidator,
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _limiteClienteRepository = limiteClienteRepository;
        _limiteClienteValidator = limiteClienteValidator;
        _unitOfWork = unitOfWork;
    }

    public async Task<CadastrarLimiteOutput> Execute(
        CadastrarLimiteInput input,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Cadastrando limite para o cliente {Documento}.", input.Documento);

        if (await _limiteClienteRepository.GetByIdAsync(input.Documento, cancellationToken) != null)
        {
            _logger.LogError("LimiteCliente já possui limite cadastrado.");

            return new CadastrarLimiteOutput(
                false,
                "LimiteCliente já possui limite cadastrado."
            );
        }

        _logger.LogInformation("LimiteCliente não possui limite cadastrado.");

        if (!_limiteClienteValidator.Validate(
                input.Documento,
                input.NumeroAgencia,
                input.NumeroConta,
                input.LimiteTransacao)
            )
        {
            _logger.LogError("Documento, agência e conta devem ser informados.");

            return new CadastrarLimiteOutput(
                false,
                "Documento, agência e conta devem ser informados."
            );
        }

        var limiteCliente = new LimiteCliente(
            input.Documento,
            input.NumeroAgencia,
            input.NumeroConta,
            input.LimiteTransacao);

        _logger.LogInformation(
            "Criando limite de {LimiteTransacao} para o cliente {Documento}.",
            input.LimiteTransacao,
            input.Documento);

        await _limiteClienteRepository.CreateAsync(limiteCliente, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        _logger.LogInformation("Limite cadastrado com sucesso.");

        return new CadastrarLimiteOutput(
            true,
            "Limite cadastrado com sucesso."
        );
    }
}
