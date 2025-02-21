namespace FraudSys.Infra.AWS.DynamoDB.Repository;

public class LimiteClienteRepository : ILimiteClienteRepository
{
    private readonly IDynamoDBContext _dynamoDbContext;

    public LimiteClienteRepository(IDynamoDBContext dynamoDbContext)
    {
        _dynamoDbContext = dynamoDbContext;
    }

    public async Task<LimiteCliente> CreateAsync(LimiteCliente input, CancellationToken cancellationToken)
    {
        return await ExecuteWithExceptionHandling(async () =>
        {
            var limiteClienteModel = LimiteClienteModel.EntityToModel(input);

            if (await ExistsAsync(limiteClienteModel.Documento, cancellationToken))
            {
                throw new FoundException($"LimiteCliente {limiteClienteModel.Documento} já existe.");
            }

            await _dynamoDbContext.SaveAsync(limiteClienteModel, cancellationToken);
            return input;
        });
    }

    public async Task<LimiteCliente> UpdateAsync(LimiteCliente input, CancellationToken cancellationToken)
    {
        return await ExecuteWithExceptionHandling(async () =>
        {
            var limiteClienteModel = LimiteClienteModel.EntityToModel(input);

            if (!await ExistsAsync(limiteClienteModel.Documento, cancellationToken))
            {
                throw new NotFoundException($"LimiteCliente {limiteClienteModel.Documento} não encontrado para atualização.");
            }

            await _dynamoDbContext.SaveAsync(limiteClienteModel, cancellationToken);
            return input;
        });
    }

    public async Task<LimiteCliente> DeleteAsync(string id, CancellationToken cancellationToken)
    {
        return await ExecuteWithExceptionHandling(async () =>
        {
            var limiteClienteModel = await GetModelByIdAsync(id, cancellationToken);

            await _dynamoDbContext.DeleteAsync(limiteClienteModel, cancellationToken);
            return LimiteClienteModel.ModelToEntity(limiteClienteModel);
        });
    }

    public async Task<LimiteCliente> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        return await ExecuteWithExceptionHandling(async () =>
        {
            var limiteClienteModel = await GetModelByIdAsync(id, cancellationToken);
            return LimiteClienteModel.ModelToEntity(limiteClienteModel);
        });
    }

    public async Task<IEnumerable<LimiteCliente>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await ExecuteWithExceptionHandling(async () =>
        {
            var search = _dynamoDbContext.ScanAsync<LimiteClienteModel>(new List<ScanCondition>());
            var result = await search.GetRemainingAsync(cancellationToken);
            return result.ConvertAll(LimiteClienteModel.ModelToEntity);
        });
    }

    public async Task<bool> ExistsAsync(string documento, CancellationToken cancellationToken)
    {
        return await ExecuteWithExceptionHandling(async () =>
        {
            return await _dynamoDbContext.LoadAsync<LimiteClienteModel>(documento, cancellationToken) != null;
        });
    }

    private async Task<LimiteClienteModel> GetModelByIdAsync(string id, CancellationToken cancellationToken)
    {
        var limiteClienteModel = await _dynamoDbContext.LoadAsync<LimiteClienteModel>(id, cancellationToken);
        if (limiteClienteModel == null)
        {
            throw new NotFoundException($"LimiteCliente {id} não encontrado.");
        }

        return limiteClienteModel;
    }

    private static async Task<T> ExecuteWithExceptionHandling<T>(Func<Task<T>> func)
    {
        try
        {
            return await func();
        }
        catch (System.Exception ex)
        {
            throw new DatabaseException("An error occurred while accessing the database.", ex);
        }
    }
}