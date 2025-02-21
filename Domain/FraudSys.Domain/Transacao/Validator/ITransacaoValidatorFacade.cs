namespace FraudSys.Domain.Transacao.Validator;

public interface ITransacaoValidatorFacade
{
    void ValidateTransacao(decimal valorTransferencia, LimiteClienteEntity limiteCliente);
}