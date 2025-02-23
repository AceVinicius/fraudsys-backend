namespace FraudSys.Application.Command.AtualizarLimite;

public record AtualizarLimiteInput(
    string Documento,
    decimal NovoLimite
);