namespace FraudSys.Domain.LimiteCliente.Validator;

public static class LimiteClienteSuficienteValidator
{
    public static void Validate(decimal valorTransferencia, LimiteClienteEntity limiteCliente)
    {
        if (valorTransferencia > limiteCliente.LimiteTransacao)
        {
            throw new EntityValidationException("Limite insuficiente para realizar a transação");
        }
    }
}