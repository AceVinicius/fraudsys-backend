namespace FraudSys.Application.Query.BuscarTodosLimites;

public record BuscarTodosLimitesOutput(IEnumerable<LimiteClienteEntity> data);