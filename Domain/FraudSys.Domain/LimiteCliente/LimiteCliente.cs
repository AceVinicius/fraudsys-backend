namespace FraudSys.Domain.LimiteCliente;

public class LimiteCliente
{
    private string Documento { get; set; }
    private string NumeroAgencia { get; set; }
    private string NumeroConta { get; set; }
    private decimal LimiteTransacao { get; set; }

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