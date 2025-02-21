namespace FraudSys.Domain.SeedWork;

public interface IRepository<TInput, in TId>
{
    public Task<TInput> CreateAsync(TInput input, CancellationToken cancellationToken);
    public Task<TInput> UpdateAsync(TInput input, CancellationToken cancellationToken);
    public Task<TInput?> DeleteAsync(TId id, CancellationToken cancellationToken);

    public Task<TInput?> GetByIdAsync(TId id, CancellationToken cancellationToken);
    public Task<IEnumerable<TInput>> GetAllAsync(CancellationToken cancellationToken);
}
