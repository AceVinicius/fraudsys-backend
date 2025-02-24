namespace FraudSys.Application.Command.EfetuarTransacao;

public record EfetuarTransacaoOutput(
    Guid Id,
    StatusTransacao Status,
    string FromDocumento,
    string FromNumeroAgencia,
    string FromNumeroConta,
    decimal FromLimiteTransacao,
    string ToDocumento,
    string ToNumeroAgencia,
    string ToNumeroConta,
    decimal ToLimiteTransacao,
    decimal ValorTransferencia,
    DateTime DataTransacao
)
{
    public EfetuarTransacaoOutput(TransacaoEntity transacao)
        : this(
            transacao.Id,
            transacao.Status,
            transacao.LimiteClientePagador.Documento,
            transacao.LimiteClientePagador.NumeroAgencia,
            transacao.LimiteClientePagador.NumeroConta,
            transacao.LimiteClientePagador.LimiteTransacao,
            transacao.LimiteClienteRecebedor.Documento,
            transacao.LimiteClienteRecebedor.NumeroAgencia,
            transacao.LimiteClienteRecebedor.NumeroConta,
            transacao.LimiteClienteRecebedor.LimiteTransacao,
            transacao.Valor,
            transacao.DataTransacao
        )
    {

    }
}