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

        if (valor <= 0)
        {
            throw new EntityCreationException("Valor da transação deve ser maior que zero.");
        }

        Valor = valor;
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
            LimiteClientePagador.Debitar(Valor);
            LimiteClienteRecebedor.Creditar(Valor);
        }
        catch (TransactionException e)
        {
            Status = StatusTransacao.Rejeitada;
            return;
        }

        Status = StatusTransacao.Aprovada;
    }
}