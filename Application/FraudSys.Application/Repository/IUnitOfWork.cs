namespace FraudSys.Application.Repository;

public interface IUnitOfWork
{
    public Task AddTransaction(object item);
    public Task CommitAsync(CancellationToken cancellationToken);
}
