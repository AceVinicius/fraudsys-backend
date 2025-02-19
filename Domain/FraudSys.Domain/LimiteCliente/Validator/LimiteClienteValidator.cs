namespace FraudSys.Domain.LimiteCliente.Validator;

public class LimiteClienteValidator : ILimiteClienteValidator
{
    public bool Validate(string documento, string numeroAgencia, string numeroConta, decimal limiteTransacao)
    {
        if (string.IsNullOrEmpty(documento))
        {
            return false;
        }

        if (string.IsNullOrEmpty(numeroAgencia))
        {
            return false;
        }

        if (string.IsNullOrEmpty(numeroConta))
        {
            return false;
        }

        if (limiteTransacao <= 0)
        {
            return false;
        }

        return true;
    }
}
