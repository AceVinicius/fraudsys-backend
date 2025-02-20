namespace FraudSys.Domain.LimiteCliente.Validator;

public class AtualizarLimiteClienteValidator: IAtualizarLimiteClienteValidator
{
    public bool Validate(decimal limiteTransacao)
    {
        return limiteTransacao >= 0;
    }
}