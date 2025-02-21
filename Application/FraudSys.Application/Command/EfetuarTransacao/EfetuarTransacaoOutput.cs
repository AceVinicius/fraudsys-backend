using FraudSys.Domain.Transacao;
using FraudSys.Domain.Transacao.Enum;

namespace FraudSys.Application.Command.EfetuarTransacao;

public record EfetuarTransacaoOutput(
    string IdTransacao,
    StatusTransacao StatusTransacao,
    string DocumentoPagador,
    string DocumentoRecebedor,
    decimal ValorTransacao,
    DateTime DataTransacao
)
{
    public EfetuarTransacaoOutput(TransacaoEntity transacao)
        : this(
            transacao.Id.ToString(),
            transacao.Status,
            transacao.LimiteClientePagador.Documento,
            transacao.LimiteClienteRecebedor.Documento,
            transacao.Valor,
            transacao.DataTransacao
        )
    {
    }
}