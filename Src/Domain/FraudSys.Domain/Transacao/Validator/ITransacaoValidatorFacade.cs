namespace FraudSys.Domain.Transacao.Validator;

public interface ITransacaoValidatorFacade
{
    void Validate(
        LimiteClienteEntity limiteClientePagador,
        LimiteClienteEntity limiteClienteRecebedor,
        decimal valor);

    void ValidateHydration(
        string id,
        int status,
        LimiteClienteEntity limiteClientePagador,
        LimiteClienteEntity limiteClienteRecebedor,
        decimal valor,
        DateTime dataTransacao);

    void ValidateEfetuarTransacao(StatusTransacao status);
}