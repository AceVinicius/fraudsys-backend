namespace FraudSys.Application.Command.CadastrarLimite;

public record CadastrarLimiteInput(
    string Documento,
    string NumeroAgencia,
    string NumeroConta,
    decimal LimiteTransacao
);