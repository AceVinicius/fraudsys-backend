namespace FraudSys.Domain.LimiteCliente;

public class LimiteCliente
{
    public string Documento { get; private set; }
    public string NumeroAgencia { get; private set; }
    public string NumeroConta { get; private set; }
    public decimal LimiteTransacao { get; private set; }

    public  LimiteCliente(string documento, string numeroAgencia, string numeroConta, decimal
            limiteTransacao)
    {
        Documento = documento;
        NumeroAgencia = numeroAgencia;
        NumeroConta = numeroConta;
        LimiteTransacao = limiteTransacao;
    }

    public void AtualizarLimite(decimal limiteTransacao)
    {
        if (limiteTransacao <= 0)
        {
            throw new ArgumentException("O limite de transação deve ser maior que zero.");
        }

        LimiteTransacao = limiteTransacao;
    }
}