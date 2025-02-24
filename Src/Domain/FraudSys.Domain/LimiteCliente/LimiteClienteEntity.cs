namespace FraudSys.Domain.LimiteCliente;

public class LimiteClienteEntity
{
    private readonly ILimiteClienteValidatorFacade _limiteClienteValidatorFacade;
    public string Documento { get; private set; }
    public string NumeroAgencia { get; private set; }
    public string NumeroConta { get; private set; }
    public decimal LimiteTransacao { get; private set; }

    private LimiteClienteEntity(
        ILimiteClienteValidatorFacade limiteClienteValidatorFacade,
        string documento,
        string numeroAgencia,
        string numeroConta,
        decimal limiteTransacao)
    {
        _limiteClienteValidatorFacade = limiteClienteValidatorFacade;
        Documento = documento;
        NumeroAgencia = numeroAgencia;
        NumeroConta = numeroConta;
        LimiteTransacao = limiteTransacao;
    }

    public void AtualizarLimite(decimal limiteTransacao)
    {
        _limiteClienteValidatorFacade.ValidateAtualizacaoLimiteCliente(limiteTransacao);

        LimiteTransacao = limiteTransacao;
    }

    public void Debitar(decimal valorDebito)
    {
        try
        {
            _limiteClienteValidatorFacade.ValidateDebito(valorDebito, LimiteTransacao);
        }
        catch (EntityValidationException ex)
        {
            throw new TransactionException(
                $"Erro ao debitar valor da transação: {ex.Message}",
                ex);
        }

        LimiteTransacao -= valorDebito;
    }

    public void Creditar(decimal valorCredito)
    {
        try
        {
            _limiteClienteValidatorFacade.ValidateCredito(valorCredito);
        }
        catch (EntityValidationException ex)
        {
            throw new TransactionException(
                $"Erro ao debitar valor da transação: {ex.Message}",
                ex);
        }

        LimiteTransacao += valorCredito;
    }

    public static LimiteClienteEntity Create(
        ILimiteClienteValidatorFacade limiteClienteValidatorFacade,
        string documento,
        string numeroAgencia,
        string numeroConta,
        decimal limiteTransacao)
    {
        limiteClienteValidatorFacade.Validate(
            documento,
            numeroAgencia,
            numeroConta,
            limiteTransacao);

        return new LimiteClienteEntity(
            limiteClienteValidatorFacade,
            documento,
            numeroAgencia,
            numeroConta,
            limiteTransacao);
    }

    public static LimiteClienteEntity Hydrate(
        ILimiteClienteValidatorFacade limiteClienteValidatorFacade,
        string documento,
        string numeroAgencia,
        string numeroConta,
        decimal limiteTransacao)
    {
        limiteClienteValidatorFacade.ValidateHydration(
            documento,
            numeroAgencia,
            numeroConta,
            limiteTransacao);

        return new LimiteClienteEntity(
            limiteClienteValidatorFacade,
            documento,
            numeroAgencia,
            numeroConta,
            limiteTransacao);
    }
}