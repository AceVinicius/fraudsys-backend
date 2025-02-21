namespace FraudSys.Application.Command.EfetuarTransacao;

public record EfetuarTransacaoInput(
    string DocumentoPagador,
    string DocumentoRecebedor,
    decimal ValorTransacao
);