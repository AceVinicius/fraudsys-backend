namespace FraudSys.Application.Command.CadastrarLimite;

public class CadastrarLimiteUseCase : ICadastrarLimiteUseCase
{
    private readonly IAppLogger<CadastrarLimiteUseCase> _appLogger;
    private readonly ILimiteClienteValidatorFacade _validatorFacade;
    private readonly ILimiteClienteRepository _limiteClienteRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CadastrarLimiteUseCase(
        IAppLogger<CadastrarLimiteUseCase> appLogger,
        ILimiteClienteValidatorFacade validatorFacade,
        ILimiteClienteRepository limiteClienteRepository,
        IUnitOfWork unitOfWork)
    {
        _appLogger = appLogger;
        _validatorFacade = validatorFacade;
        _limiteClienteRepository = limiteClienteRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CadastrarLimiteOutput> Execute(
        CadastrarLimiteInput input,
        CancellationToken cancellationToken)
    {
        _appLogger.LogInformation($"Iniciando o use case para cadastro do LimiteCliente '{input.Documento}'.");

        var limiteCliente = LimiteClienteEntity.Create(
            _validatorFacade,
            input.Documento,
            input.NumeroAgencia,
            input.NumeroConta,
            input.LimiteTransacao);

        await _limiteClienteRepository.CreateAsync(limiteCliente, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        _appLogger.LogInformation("LimiteCliente cadastrado com sucesso.");

        return new CadastrarLimiteOutput(limiteCliente);
    }
}
