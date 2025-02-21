namespace FraudSys.Application.Repository;

public interface IUnitOfWork
{
    public Task AddTransaction(object item);
    public Task<bool> CommitAsync(CancellationToken cancellationToken);
}
