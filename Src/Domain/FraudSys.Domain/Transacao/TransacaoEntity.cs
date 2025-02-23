namespace FraudSys.Domain.Transacao;

public class TransacaoEntity
{
    private readonly ITransacaoValidatorFacade _transacaoValidatorFacade;
    public Guid Id { get; private set; }
    public StatusTransacao Status { get; private set; }
    public LimiteClienteEntity LimiteClientePagador { get; private set; }
    public LimiteClienteEntity LimiteClienteRecebedor { get; private set; }
    public decimal Valor { get; private set; }
    public DateTime DataTransacao { get; private set; }

    private TransacaoEntity(
        ITransacaoValidatorFacade transacaoValidatorFacade,
        Guid id,
        StatusTransacao status,
        LimiteClienteEntity limiteClientePagador,
        LimiteClienteEntity limiteClienteRecebedor,
        decimal valor,
        DateTime dataTransacao)
    {
        _transacaoValidatorFacade = transacaoValidatorFacade;
        Id = id;
        Status = status;
        LimiteClientePagador = limiteClientePagador;
        LimiteClienteRecebedor = limiteClienteRecebedor;
        Valor = valor;
        DataTransacao = dataTransacao;
    }

    public void EfetuarTransacao()
    {
        _transacaoValidatorFacade.ValidateEfetuarTransacao(Status);

        try
        {
            LimiteClientePagador.Debitar(Valor);
            LimiteClienteRecebedor.Creditar(Valor);

            Status = StatusTransacao.Aprovada;
        }
        catch (TransactionException)
        {
            Status = StatusTransacao.Rejeitada;
            throw;
        }
    }

    public static TransacaoEntity Create(
        ITransacaoValidatorFacade transacaoValidatorFacade,
        LimiteClienteEntity limiteClientePagador,
        LimiteClienteEntity limiteClienteRecebedor,
        decimal valor)
    {
        transacaoValidatorFacade.Validate(
            limiteClientePagador,
            limiteClienteRecebedor,
            valor);

        return new TransacaoEntity(
            transacaoValidatorFacade,
            Guid.NewGuid(),
            StatusTransacao.Pendente,
            limiteClientePagador,
            limiteClienteRecebedor,
            valor,
            DateTime.Now);
    }

    public static TransacaoEntity Hydrate(
        ITransacaoValidatorFacade transacaoValidatorFacade,
        string id,
        int status,
        LimiteClienteEntity limiteClientePagador,
        LimiteClienteEntity limiteClienteRecebedor,
        decimal valor,
        DateTime dataTransacao)
    {
        transacaoValidatorFacade.ValidateHydration(
            id,
            status,
            limiteClientePagador,
            limiteClienteRecebedor,
            valor,
            dataTransacao);

        var parsedId = Guid.Parse(id);
        var parsedStatus = (StatusTransacao)status;
        var parsedDataTransacao = dataTransacao;

        return new TransacaoEntity(
            transacaoValidatorFacade,
            parsedId,
            parsedStatus,
            limiteClientePagador,
            limiteClienteRecebedor,
            valor,
            parsedDataTransacao);
    }
}