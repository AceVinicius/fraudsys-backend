namespace FraudSys.Domain.LimiteCliente.Validator;

public static class LimiteClienteValidator
{
    public static void Validate(decimal limiteTransacao)
    {
        if (limiteTransacao < 0)
        {
            throw new EntityValidationException("Limite de transação não pode ser menor que 0.");
        }
    }
}