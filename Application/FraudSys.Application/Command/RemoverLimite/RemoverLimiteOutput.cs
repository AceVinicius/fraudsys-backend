namespace FraudSys.Application.Command.RemoverLimite;

public record RemoverLimiteOutput(
    string Documento,
    string NumeroAgencia,
    string NumeroConta,
    decimal LimiteTransacao
)
{
    public RemoverLimiteOutput(LimiteCliente limiteCliente)
        : this(
            limiteCliente.Documento,
            limiteCliente.NumeroAgencia,
            limiteCliente.NumeroConta,
            limiteCliente.LimiteTransacao
        )
    {
    }
}