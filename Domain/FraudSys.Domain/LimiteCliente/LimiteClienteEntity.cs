namespace FraudSys.Domain.LimiteCliente;

public class LimiteClienteEntity
{
    public string Documento { get; private set; }
    public string NumeroAgencia { get; private set; }
    public string NumeroConta { get; private set; }
    public decimal LimiteTransacao { get; private set; }

    public  LimiteClienteEntity(string documento, string numeroAgencia, string numeroConta, decimal
            limiteTransacao)
    {
        Documento = documento;
        NumeroAgencia = numeroAgencia;
        NumeroConta = numeroConta;

        if (limiteTransacao < 0)
        {
            throw new EntityCreationException("O limite de transação deve ser maior que 0.");
        }

        LimiteTransacao = limiteTransacao;
    }

    public void AtualizarLimite(decimal limiteTransacao)
    {
        if (limiteTransacao < 0)
        {
            throw new EntityCreationException("O limite de transação deve ser maior que 0.");
        }

        LimiteTransacao = limiteTransacao;
    }

    public void Debitar(decimal valorDebito)
    {
        if (valorDebito < 0)
        {
            throw new TransactionException("O valor da transação deve ser maior que 0.");
        }

        if (valorDebito > LimiteTransacao)
        {
            throw new TransactionException("O valor da transação é maior que o limite disponível.");
        }

        LimiteTransacao -= valorDebito;
    }

    public void Creditar(decimal valorCredito)
    {
        if (valorCredito < 0)
        {
            throw new TransactionException("O valor da transação deve ser maior que 0.");
        }

        LimiteTransacao += valorCredito;
    }
}