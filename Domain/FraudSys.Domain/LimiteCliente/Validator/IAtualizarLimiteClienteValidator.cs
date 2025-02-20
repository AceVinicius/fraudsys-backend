namespace FraudSys.Domain.LimiteCliente.Validator;

public interface IAtualizarLimiteClienteValidator
{
    public bool Validate(decimal limiteTransacao);
}