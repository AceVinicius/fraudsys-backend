namespace FraudSys.Domain.LimiteCliente.Repository;

public interface ILimiteClienteRepository : IRepository<LimiteCliente, string>
{
    Task<bool> ExistsAsync(string documento, CancellationToken cancellationToken);
}