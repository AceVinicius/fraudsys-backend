namespace FraudSys.Application.Query.BuscarLimite;

public record BuscarLimiteOutput(
    string Documento,
    string NumeroAgencia,
    string NumeroConta,
    decimal LimiteTransacao
)
{
    public BuscarLimiteOutput(LimiteClienteEntity limiteClienteEntity)
        : this(
            limiteClienteEntity.Documento,
            limiteClienteEntity.NumeroAgencia,
            limiteClienteEntity.NumeroConta,
            limiteClienteEntity.LimiteTransacao
        )
    {
    }
}