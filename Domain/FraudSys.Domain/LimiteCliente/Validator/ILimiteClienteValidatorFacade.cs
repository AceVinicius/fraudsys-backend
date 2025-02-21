namespace FraudSys.Domain.LimiteCliente.Validator;

public interface ILimiteClienteValidatorFacade
{
    void ValidateCriacaoLimiteCliente(
        string documento,
        string numeroAgencia,
        string numeroConta,
        decimal limiteTransacao);

    void ValidateAtualizacaoLimiteCliente(decimal limiteCliente);
}