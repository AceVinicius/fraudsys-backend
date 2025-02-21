namespace FraudSys.Application.Command.CadastrarLimite;

public record CadastrarLimiteOutput(
    string Documento,
    string NumeroAgencia,
    string NumeroConta,
    decimal LimiteTransacao
)
{
    public CadastrarLimiteOutput(LimiteClienteEntity limiteClienteEntity)
        : this(
            limiteClienteEntity.Documento,
            limiteClienteEntity.NumeroAgencia,
            limiteClienteEntity.NumeroConta,
            limiteClienteEntity.LimiteTransacao
        )
    {
    }
}