namespace FraudSys.Application.Command.CadastrarLimite;

public class CadastrarLimiteUseCase : ICadastrarLimiteUseCase
{
    private readonly IAppLogger<CadastrarLimiteUseCase> _appLogger;
    private readonly ILimiteClienteRepository _limiteClienteRepository;
    private readonly ILimiteClienteValidatorFacade _validatorFacade;
    private readonly IUnitOfWork _unitOfWork;

    public CadastrarLimiteUseCase(
        IAppLogger<CadastrarLimiteUseCase> appLogger,
        ILimiteClienteRepository limiteClienteRepository,
        ILimiteClienteValidatorFacade validatorFacade,
        IUnitOfWork unitOfWork)
    {
        _appLogger = appLogger;
        _limiteClienteRepository = limiteClienteRepository;
        _validatorFacade = validatorFacade;
        _unitOfWork = unitOfWork;
    }

    public async Task<CadastrarLimiteOutput> Execute(
        CadastrarLimiteInput input,
        CancellationToken cancellationToken)
    {
        _appLogger.LogInformation($"Cadastrando limite para o cliente {input.Documento}.");

        var limiteCliente = LimiteClienteEntity.Create(
            _validatorFacade,
            input.Documento,
            input.NumeroAgencia,
            input.NumeroConta,
            input.LimiteTransacao);

        _appLogger.LogInformation($"Criando limite de {input.LimiteTransacao} para o cliente {input.Documento}.");

        await _limiteClienteRepository.CreateAsync(limiteCliente, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        _appLogger.LogInformation("Limite cadastrado com sucesso.");

        return new CadastrarLimiteOutput(limiteCliente);
    }
}
