namespace FraudSys.Domain.SeedWork;

public interface IRepository<TInput, in TId>
{
    public Task<TInput> Create(TInput input, CancellationToken cancellationToken);
    public Task<TInput> Update(TInput input, CancellationToken cancellationToken);
    public Task<TInput> Delete(TId id, CancellationToken cancellationToken);

    public Task<TInput?> GetById(TId id, CancellationToken cancellationToken);
    public Task<IEnumerable<TInput>> GetAll(CancellationToken cancellationToken);
}
