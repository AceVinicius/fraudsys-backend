using System.Transactions;

namespace FraudSys.Domain.LimiteCliente.Validator;

public class LimiteClienteValidatorFacade : ILimiteClienteValidatorFacade
{
    public void Validate(
        string documento,
        string numeroAgencia,
        string numeroConta,
        decimal limiteTransacao)
    {
        LimiteClienteValidator.ValidateEmptyString(documento);
        LimiteClienteValidator.ValidateEmptyString(numeroAgencia);
        LimiteClienteValidator.ValidateEmptyString(numeroConta);
        LimiteClienteValidator.ValidateLimiteCliente(limiteTransacao);
    }

    public void ValidateHydration(
        string documento,
        string numeroAgencia,
        string numeroConta,
        decimal limiteTransacao)
    {
        try
        {
            LimiteClienteValidator.ValidateEmptyString(documento);
            LimiteClienteValidator.ValidateEmptyString(numeroAgencia);
            LimiteClienteValidator.ValidateEmptyString(numeroConta);
            LimiteClienteValidator.ValidateLimiteCliente(limiteTransacao);
        }
        catch (EntityValidationException ex)
        {
            throw new EntityHydrationException(
                "Erro ao hidratar entidade de limite de cliente: {ex.Message}",
                ex);
        }
    }

    public void ValidateAtualizacaoLimiteCliente(decimal limiteCliente)
    {
        LimiteClienteValidator.ValidateLimiteCliente(limiteCliente);
    }

    public void ValidateCredito(decimal valorCredito)
    {
        TransacaoValidator.ValidadeValorTransacao(valorCredito);
    }

    public void ValidateDebito(decimal valorDebito, decimal limitecliente)
    {
        TransacaoValidator.ValidadeValorTransacao(valorDebito);
        LimiteClienteValidator.LimiteClienteSuficiente(valorDebito, limitecliente);
    }
}