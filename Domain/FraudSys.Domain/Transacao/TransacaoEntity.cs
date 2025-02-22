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
        Status = StatusTransacao.Pendente;
        LimiteClientePagador = limiteClientePagador;
        LimiteClienteRecebedor = limiteClienteRecebedor;
        Valor = valor;
        DataTransacao = DateTime.Now;
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
        if (Status != StatusTransacao.Pendente)
        {
            return;
        }

        if (LimiteClientePagador == LimiteClienteRecebedor)
        {
            Status = StatusTransacao.Rejeitada;
            return;
        }

        if (Valor <= 0)
        {
            Status = StatusTransacao.Rejeitada;
            return;
        }

        try
        {
            LimiteClientePagador.Debitar(Valor);
            LimiteClienteRecebedor.Creditar(Valor);
        }
        catch (TransactionException)
        {
            Status = StatusTransacao.Rejeitada;
            return;
        }

        Status = StatusTransacao.Aprovada;
    }
}