namespace FraudSys.Domain.LimiteCliente.Validator;

public interface ICadastrarLimiteClienteValidator
{
    public bool Validate(string documento, string numeroAgencia, string numeroConta, decimal limiteTransacao);
}