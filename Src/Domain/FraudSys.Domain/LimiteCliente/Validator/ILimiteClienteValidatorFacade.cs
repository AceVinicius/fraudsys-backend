namespace FraudSys.Domain.LimiteCliente.Validator;

public interface ILimiteClienteValidatorFacade
{
    void Validate(
        string documento,
        string numeroAgencia,
        string numeroConta,
        decimal limiteTransacao);
    void ValidateHydration(
        string documento,
        string numeroAgencia,
        string numeroConta,
        decimal limiteTransacao);
    void ValidateAtualizacaoLimiteCliente(decimal limiteCliente);
    void ValidateCredito(decimal valorCredito);
    void ValidateDebito(decimal valorDebito, decimal limiteTransacao);
}