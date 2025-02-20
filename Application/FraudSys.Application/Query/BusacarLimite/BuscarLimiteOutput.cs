namespace FraudSys.Application.Query.BusacarLimite;

public record BuscarLimiteOutput(
    string Documento,
    string NumeroAgencia,
    string NumeroConta,
    decimal LimiteTransacao
);