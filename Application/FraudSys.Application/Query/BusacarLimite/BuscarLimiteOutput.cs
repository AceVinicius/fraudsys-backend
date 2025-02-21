namespace FraudSys.Application.Query.BusacarLimite;

public record BuscarLimiteOutput(
    string Documento,
    string NumeroAgencia,
    string NumeroConta,
    decimal LimiteTransacao
)
{
    public BuscarLimiteOutput(LimiteCliente limiteCliente)
        : this(
            limiteCliente.Documento,
            limiteCliente.NumeroAgencia,
            limiteCliente.NumeroConta,
            limiteCliente.LimiteTransacao
        )
    {
    }
}