namespace FraudSys.Infra.AWS.DynamoDB;

public sealed class UnitOfWork : IUnitOfWork
{
    public UnitOfWork() {}

    public Task CommitAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task RollbackAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}