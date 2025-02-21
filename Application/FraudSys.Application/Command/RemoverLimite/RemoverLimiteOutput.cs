namespace FraudSys.Application.Command.RemoverLimite;

public record RemoverLimiteOutput(
    string Documento,
    string NumeroAgencia,
    string NumeroConta,
    decimal LimiteTransacao
)
{
    public RemoverLimiteOutput(LimiteClienteEntity limiteClienteEntity)
        : this(
            limiteClienteEntity.Documento,
            limiteClienteEntity.NumeroAgencia,
            limiteClienteEntity.NumeroConta,
            limiteClienteEntity.LimiteTransacao
        )
    {
    }
}