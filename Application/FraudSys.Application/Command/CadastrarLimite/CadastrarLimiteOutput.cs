namespace FraudSys.Application.Command.CadastrarLimite;

public record CadastrarLimiteOutput(
    string Documento,
    string NumeroAgencia,
    string NumeroConta,
    decimal LimiteTransacao
)
{
    public CadastrarLimiteOutput(LimiteCliente limiteCliente)
        : this(
            limiteCliente.Documento,
            limiteCliente.NumeroAgencia,
            limiteCliente.NumeroConta,
            limiteCliente.LimiteTransacao
        )
    {
    }
}