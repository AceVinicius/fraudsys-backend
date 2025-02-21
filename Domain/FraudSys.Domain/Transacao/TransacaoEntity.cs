using FraudSys.Domain.Transacao.Enum;

namespace FraudSys.Domain.Transacao;

public class TransacaoEntity
{
    public Guid Id { get; private set; }
    public StatusTransacao Status { get; private set; }
    public LimiteClienteEntity LimiteClientePagador { get; private set; }
    public LimiteClienteEntity LimiteClienteRecebedor { get; private set; }
    public decimal Valor { get; private set; }
    public DateTime DataTransacao { get; private set; }

    public TransacaoEntity(
        LimiteClienteEntity limiteClientePagador,
        LimiteClienteEntity limiteClienteRecebedor,
        decimal valor)
    {
        Id = Guid.NewGuid();
        LimiteClientePagador = limiteClientePagador;
        LimiteClienteRecebedor = limiteClienteRecebedor;
        Valor = valor;
        Status = StatusTransacao.Pendente;
    }

    public TransacaoEntity(
        Guid id,
        StatusTransacao status,
        LimiteClienteEntity limiteClientePagador,
        LimiteClienteEntity limiteClienteRecebedor,
        decimal valor,
        DateTime dataTransacao)
    {
        Id = id;
        Status = status;
        LimiteClientePagador = limiteClientePagador;
        LimiteClienteRecebedor = limiteClienteRecebedor;
        Valor = valor;
        DataTransacao = dataTransacao;
    }

    public void EfetuarTransacao()
    {
        DataTransacao = DateTime.Now;

        try
        {
            LimiteClienteRecebedor.Debitar(Valor);
            LimiteClientePagador.Creditar(Valor);
        }
        catch (TransactionException e)
        {
            Status = StatusTransacao.Rejeitada;
        }

        Status = StatusTransacao.Aprovada;
    }
}