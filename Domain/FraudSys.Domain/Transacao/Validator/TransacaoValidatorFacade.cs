namespace FraudSys.Domain.Transacao.Validator;

public class TransacaoValidatorFacade : ITransacaoValidatorFacade
{
    public void ValidateTransacao(decimal valorTransferencia, LimiteClienteEntity limiteCliente)
    {
        ValorTransferenciaValidator.Validate(valorTransferencia);
        LimiteClienteSuficienteValidator.Validate(valorTransferencia, limiteCliente);
    }
}