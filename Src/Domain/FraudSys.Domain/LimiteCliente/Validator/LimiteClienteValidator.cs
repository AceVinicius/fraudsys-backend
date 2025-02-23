namespace FraudSys.Domain.LimiteCliente.Validator;

public static class LimiteClienteValidator
{
    public static void ValidateEmptyString(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new EntityValidationException("O valor não pode ser nulo ou vazio.");
        }
    }

    public static void ValidateLimiteCliente(decimal value)
    {
        if (value < 0)
        {
            throw new EntityValidationException("O valor do limite de transação deve ser maior que 0.");
        }
    }

    public static void LimiteClienteSuficiente(decimal valorDebito, decimal limiteCliente)
    {
        if (valorDebito > limiteCliente)
        {
            throw new EntityValidationException("O valor da transação é maior que o limite disponível.");
        }
    }
}