namespace FraudSys.Domain.Transacao.Validator;

public static class ValorTransferenciaValidator
{
    public static void Validate(decimal valorTransferencia)
    {
        if (valorTransferencia <= 0)
        {
            throw new EntityValidationException("Valor da transferência deve ser maior que zero.");
        }
    }
}