namespace FraudSys.Application.Command.AtualizarLimite;

public record AtualizarLimiteOutput(
    string Documento,
    string NumeroAgencia,
    string NumeroConta,
    decimal LimiteTransacao
)
{
    public AtualizarLimiteOutput(LimiteClienteEntity limiteCliente)
        : this(
            limiteCliente.Documento,
            limiteCliente.NumeroAgencia,
            limiteCliente.NumeroConta,
            limiteCliente.LimiteTransacao
        )
    {
    }
}
