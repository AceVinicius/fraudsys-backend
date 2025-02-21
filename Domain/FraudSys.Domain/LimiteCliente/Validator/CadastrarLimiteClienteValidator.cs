namespace FraudSys.Domain.LimiteCliente.Validator;

public static class CadastrarLimiteClienteValidator
{
    public static void Validate(
        string documento,
        string numeroAgencia,
        string numeroConta)
    {
        if (string.IsNullOrWhiteSpace(documento))
        {
            throw new EntityValidationException("O campo documento é obrigatório.");
        }

        if (string.IsNullOrWhiteSpace(numeroAgencia))
        {
            throw new EntityValidationException("O campo número da agência é obrigatório.");
        }

        if (string.IsNullOrWhiteSpace(numeroConta))
        {
            throw new EntityValidationException("O campo número da conta é obrigatório.");
        }
    }
}
