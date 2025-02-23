namespace FraudSys.Domain.LimiteCliente.Validator;

public class LimiteClienteValidatorFacade : ILimiteClienteValidatorFacade
{
    public void ValidateCriacaoLimiteCliente(
        string documento,
        string numeroAgencia,
        string numeroConta,
        decimal limiteTransacao)
    {
        CadastrarLimiteClienteValidator.Validate(documento, numeroAgencia, numeroConta);
        LimiteClienteValidator.Validate(limiteTransacao);
    }

    public void ValidateAtualizacaoLimiteCliente(decimal limiteCliente)
    {
        LimiteClienteValidator.Validate(limiteCliente);
    }
}