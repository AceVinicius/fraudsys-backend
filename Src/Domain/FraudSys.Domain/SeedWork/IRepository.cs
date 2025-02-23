namespace FraudSys.Domain.SeedWork;

public interface IRepository<TInput, in TId>
{
    Task CreateAsync(TInput input, CancellationToken cancellationToken);
    Task UpdateAsync(TInput input, CancellationToken cancellationToken);
    Task DeleteAsync(TId id, CancellationToken cancellationToken);

    Task<bool> ExistsAsync(TId id, CancellationToken cancellationToken);

    Task<TInput> GetByIdAsync(TId id, CancellationToken cancellationToken);
    Task<IEnumerable<TInput>> GetAllAsync(CancellationToken cancellationToken);
}
