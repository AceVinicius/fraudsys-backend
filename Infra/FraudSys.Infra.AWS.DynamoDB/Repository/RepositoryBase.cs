namespace FraudSys.Infra.AWS.DynamoDB.Repository;

public abstract class RepositoryBase<TRepository, TEntity, TModel, TId> : IRepository<TEntity, TId>
    where TModel : class, IModel<TModel, TEntity, TId>, new()
{
    protected readonly IAppLogger<TRepository> AppLogger;
    protected readonly IAmazonDynamoDB Client;
    protected readonly IUnitOfWork UnitOfWork;
    protected readonly string TableName;

    protected RepositoryBase(
        IAppLogger<TRepository> appLogger,
        IAmazonDynamoDB client,
        IUnitOfWork unitOfWork,
        string tableName)
    {
        AppLogger = appLogger;
        Client = client;
        UnitOfWork = unitOfWork;
        TableName = tableName;
    }

    public async Task CreateAsync(TEntity input, CancellationToken cancellationToken)
    {
        await ExecuteWithExceptionHandling(async () =>
        {
            AppLogger.LogInformation($"Creating entity '{input}'.");

            var model = new TModel().EntityToModel(input);

            if (await ExistsAsync(new TModel().GetIdFromEntity(input), cancellationToken))
            {
                throw new FoundException($"Entity '{new TModel().GetIdFromEntity(input)}' already exists.");
            }

            await UnitOfWork.AddTransaction(new TransactWriteItem
            {
                Put = new Put
                {
                    TableName = TableName,
                    Item = new TModel().ToAttributeMap(model),
                    ConditionExpression = "attribute_not_exists(Id)"
                }
            });

            AppLogger.LogInformation($"Entity '{new TModel().GetIdFromEntity(input)}' added to transactions.");
        });
    }

    public async Task UpdateAsync(TEntity input, CancellationToken cancellationToken)
    {
        await ExecuteWithExceptionHandling(async () =>
        {
            AppLogger.LogInformation($"Updating entity '{input}'.");

            var model = new TModel().EntityToModel(input);

            if (!await ExistsAsync(new TModel().GetIdFromEntity(input), cancellationToken))
            {
                throw new NotFoundException($"Entity '{new TModel().GetIdFromEntity(input)}' does not exist.");
            }

            await UnitOfWork.AddTransaction(new TransactWriteItem
            {
                Put = new Put
                {
                    TableName = TableName,
                    Item = new TModel().ToAttributeMap(model)
                }
            });

            AppLogger.LogInformation($"Entity '{new TModel().GetIdFromEntity(input)}' added to transactions.");
        });
    }

    public async Task DeleteAsync(TId id, CancellationToken cancellationToken)
    {
        await ExecuteWithExceptionHandling(async () =>
        {
            AppLogger.LogInformation($"Deleting entity '{id}'.");

            if (!await ExistsAsync(id, cancellationToken))
            {
                throw new NotFoundException($"Entity '{id}' does not exist.");
            }

            await UnitOfWork.AddTransaction(new TransactWriteItem
            {
                Delete = new Delete
                {
                    TableName = TableName,
                    Key = new Dictionary<string, AttributeValue>
                        { { "Id", new AttributeValue { S = id?.ToString() } } }
                }
            });
        });
    }

    public async Task<TEntity> GetByIdAsync(TId id, CancellationToken cancellationToken)
    {
        return await ExecuteWithExceptionHandling(async () =>
        {
            AppLogger.LogInformation($"Fetching entity '{id}'.");

            var response = await Client.GetItemAsync(new GetItemRequest
            {
                TableName = TableName,
                Key = new Dictionary<string, AttributeValue> { { "Id", new AttributeValue { S = id?.ToString() } } }
            }, cancellationToken);

            if (response.Item.Count == 0)
            {
                throw new NotFoundException($"Entity '{id}' not found.");
            }

            var model = new TModel().FromAttributeMap(response.Item);

            return new TModel().ModelToEntity(model);
        });
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await ExecuteWithExceptionHandling(async () =>
        {
            AppLogger.LogInformation("Fetching all entities.");

            var request = new ScanRequest { TableName = TableName };
            var response = await Client.ScanAsync(request, cancellationToken);

            AppLogger.LogInformation($"Found {response.Count} entities.");

            return response.Items
                .Select(item => new TModel().FromAttributeMap(item))
                .Select(model => new TModel().ModelToEntity(model))
                .ToList();
        });
    }

    public async Task<bool> ExistsAsync(TId id, CancellationToken cancellationToken)
    {
        return await ExecuteWithExceptionHandling(async () =>
        {
            var response = await Client.GetItemAsync(new GetItemRequest
            {
                TableName = TableName,
                Key = new Dictionary<string, AttributeValue>
                    { { "Id", new AttributeValue { S = id?.ToString() } } }
            }, cancellationToken);

            return response.Item.Count > 0;
        });
    }

    protected static async Task ExecuteWithExceptionHandling(Func<Task> action)
    {
        try
        {
            await action();
        }
        catch (System.Exception ex)
        {
            throw new DatabaseException(ex.Message, ex);
        }
    }

    protected static async Task<T> ExecuteWithExceptionHandling<T>(Func<Task<T>> action)
    {
        return await action();

        try
        {

        }
        catch (System.Exception ex)
        {
            throw new DatabaseException(ex.Message, ex);
        }
    }
}