namespace FraudSys.Infra.AWS.DynamoDB;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly IAppLogger<UnitOfWork> _appLogger;
    private readonly IAmazonDynamoDB _client;
    private readonly List<TransactWriteItem> _transactions;

    public UnitOfWork(
        IAppLogger<UnitOfWork> appLogger,
        IAmazonDynamoDB client)
    {
        _appLogger = appLogger;
        _client = client;

        _transactions = [];
    }

    public Task AddTransaction(object item)
    {
        ArgumentNullException.ThrowIfNull(item);

        if (item.GetType() != typeof(TransactWriteItem))
        {
            throw new ArgumentException("transactWriteItem must be of type TransactWriteItem.");
        }

        _transactions.Add((TransactWriteItem) item);

        return Task.CompletedTask;
    }

    public async Task<bool> CommitAsync(CancellationToken cancellationToken)
    {
        if (_transactions.Count == 0)
            return false;

        var request = new TransactWriteItemsRequest { TransactItems = _transactions };

        try
        {
            await _client.TransactWriteItemsAsync(request, cancellationToken);
            _transactions.Clear();
            return true;
        }
        catch (System.Exception ex)
        {
            _appLogger.LogError("Erro ao executar transações.");
            return false;
        }
    }
}