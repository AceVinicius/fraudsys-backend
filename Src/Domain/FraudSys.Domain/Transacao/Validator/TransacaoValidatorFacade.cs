namespace FraudSys.Domain.Transacao.Validator;

public class TransacaoValidatorFacade : ITransacaoValidatorFacade
{
    public void Validate(
        LimiteClienteEntity limiteClientePagador,
        LimiteClienteEntity limiteClienteRecebedor,
        decimal valor)
    {
        TransacaoValidator.ValidateLimiteClienteEntity(limiteClientePagador);
        TransacaoValidator.ValidateLimiteClienteEntity(limiteClienteRecebedor);
        TransacaoValidator.ValidatePagadorERecebedor(limiteClientePagador, limiteClienteRecebedor);
        TransacaoValidator.ValidadeValorTransacao(valor);
    }

    public void ValidateHydration(
        string id,
        int status,
        LimiteClienteEntity limiteClientePagador,
        LimiteClienteEntity limiteClienteRecebedor,
        decimal valor,
        DateTime dataTransacao)
    {
        try
        {
            TransacaoValidator.ValidateId(id);
            TransacaoValidator.ValidateStatus(status);
            TransacaoValidator.ValidateLimiteClienteEntity(limiteClientePagador);
            TransacaoValidator.ValidateLimiteClienteEntity(limiteClienteRecebedor);
            TransacaoValidator.ValidatePagadorERecebedor(limiteClientePagador, limiteClienteRecebedor);
            TransacaoValidator.ValidadeValorTransacao(valor);
        }
        catch (EntityValidationException ex)
        {
            throw new EntityHydrationException(
                $"Não foi possível hidratar a entidade Transacao: {ex.Message}",
                ex);
        }
    }

    public void ValidateEfetuarTransacao(StatusTransacao status)
    {
        TransacaoValidator.ValidateStatusTransacao(status);
    }
}