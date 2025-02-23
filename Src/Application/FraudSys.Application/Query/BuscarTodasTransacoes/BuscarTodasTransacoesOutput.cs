namespace FraudSys.Application.Query.BuscarTodasTransacoes;

public record BuscarTodasTransacoesOutput(IEnumerable<TransacaoEntity> data);