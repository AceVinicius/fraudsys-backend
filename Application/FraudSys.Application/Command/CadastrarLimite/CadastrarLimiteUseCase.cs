using FraudSys.Application.Repository;

namespace FraudSys.Application.Command.CadastrarLimite;

public class CadastrarLimiteUseCase : ICadastrarLimiteUseCase
{
    private readonly IAppLogger<CadastrarLimiteUseCase> _appLogger;
    private readonly ILimiteClienteRepository _limiteClienteRepository;
    private readonly ICadastrarLimiteClienteValidator _limiteClienteValidator;
    private readonly IUnitOfWork _unitOfWork;

    public CadastrarLimiteUseCase(
        IAppLogger<CadastrarLimiteUseCase> appLogger,
        ILimiteClienteRepository limiteClienteRepository,
        ICadastrarLimiteClienteValidator limiteClienteValidator,
        IUnitOfWork unitOfWork)
    {
        _appLogger = appLogger;
        _limiteClienteRepository = limiteClienteRepository;
        _limiteClienteValidator = limiteClienteValidator;
        _unitOfWork = unitOfWork;
    }

    public async Task<CadastrarLimiteOutput> Execute(
        CadastrarLimiteInput input,
        CancellationToken cancellationToken)
    {
        _appLogger.LogInformation($"Cadastrando limite para o cliente {input.Documento}.");

        if (await _limiteClienteRepository.GetByIdAsync(input.Documento, cancellationToken) != null)
        {
            _appLogger.LogError("LimiteCliente já possui limite cadastrado.");

            return new CadastrarLimiteOutput(
                false,
                "LimiteCliente já possui limite cadastrado."
            );
        }

        _appLogger.LogInformation("LimiteCliente não possui limite cadastrado.");

        if (!_limiteClienteValidator.Validate(
                input.Documento,
                input.NumeroAgencia,
                input.NumeroConta,
                input.LimiteTransacao)
            )
        {
            _appLogger.LogError("Documento, agência e conta devem ser informados.");

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

        _appLogger.LogInformation($"Criando limite de {input.LimiteTransacao} para o cliente {input.Documento}.");

        await _limiteClienteRepository.CreateAsync(limiteCliente, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        _appLogger.LogInformation("Limite cadastrado com sucesso.");

        return new CadastrarLimiteOutput(
            true,
            "Limite cadastrado com sucesso."
        );
    }
}
