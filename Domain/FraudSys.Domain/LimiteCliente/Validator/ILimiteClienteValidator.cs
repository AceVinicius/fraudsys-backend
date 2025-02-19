namespace FraudSys.Domain.LimiteCliente.Validator;

public interface ILimiteClienteValidator
{
    public bool Validate(string documento, string numeroAgencia, string numeroConta, decimal limiteTransacao);
}