namespace FraudSys.Infra.AWS.DynamoDB.Repository;

public class LimiteClienteRepository : ILimiteClienteRepository
{
    private readonly IAppLogger<LimiteClienteRepository> _appLogger;
    private readonly IDynamoDBContext _dynamoDbContext;

    public LimiteClienteRepository(
        IAppLogger<LimiteClienteRepository> appLogger,
        IDynamoDBContext dynamoDbContext)
    {
        _appLogger = appLogger;
        _dynamoDbContext = dynamoDbContext;
    }

    public async Task<LimiteCliente> CreateAsync(LimiteCliente input, CancellationToken cancellationToken)
    {
        return await ExecuteWithExceptionHandling(async () =>
        {
            _appLogger.LogInformation($"Criando LimiteCliente '{input.Documento}'.");

            var limiteClienteModel = LimiteClienteModel.EntityToModel(input);

            if (await ExistsAsync(limiteClienteModel.Documento, cancellationToken))
            {
                var message = $"LimiteCliente '{limiteClienteModel.Documento}' já existe.";

                _appLogger.LogError(message);
                throw new FoundException(message);
            }

            _appLogger.LogInformation($"LimiteCliente '{limiteClienteModel.Documento}' não existe, criando.");

            await _dynamoDbContext.SaveAsync(limiteClienteModel, cancellationToken);

            _appLogger.LogInformation($"LimiteCliente '{limiteClienteModel.Documento}' criado com sucesso.");

            return input;
        });
    }

    public async Task<LimiteCliente> UpdateAsync(LimiteCliente input, CancellationToken cancellationToken)
    {
        return await ExecuteWithExceptionHandling(async () =>
        {
            _appLogger.LogInformation($"Atualizando LimiteCliente '{input.Documento}'.");

            var limiteClienteModel = LimiteClienteModel.EntityToModel(input);

            if (!await ExistsAsync(limiteClienteModel.Documento, cancellationToken))
            {
                var message = $"LimiteCliente '{limiteClienteModel.Documento}' não encontrado para atualização.";

                _appLogger.LogError(message);
                throw new NotFoundException(message);
            }

            _appLogger.LogInformation($"LimiteCliente '{limiteClienteModel.Documento}' encontrado, atualizando.");

            await _dynamoDbContext.SaveAsync(limiteClienteModel, cancellationToken);

            _appLogger.LogInformation($"LimiteCliente '{limiteClienteModel.Documento}' atualizado com sucesso.");

            return input;
        });
    }

    public async Task<LimiteCliente> DeleteAsync(string id, CancellationToken cancellationToken)
    {
        return await ExecuteWithExceptionHandling(async () =>
        {
            _appLogger.LogInformation($"Deletando LimiteCliente '{id}'.");

            var limiteClienteModel = await GetModelByIdAsync(id, cancellationToken);

            _appLogger.LogInformation($"LimiteCliente '{limiteClienteModel.Documento}' encontrado, deletando.");

            await _dynamoDbContext.DeleteAsync(limiteClienteModel, cancellationToken);

            _appLogger.LogInformation($"LimiteCliente '{limiteClienteModel.Documento}' deletado com sucesso.");

            return LimiteClienteModel.ModelToEntity(limiteClienteModel);
        });
    }

    public async Task<LimiteCliente> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        return await ExecuteWithExceptionHandling(async () =>
        {
            _appLogger.LogInformation($"Buscando LimiteCliente '{id}'.");

            var limiteClienteModel = await GetModelByIdAsync(id, cancellationToken);

            _appLogger.LogInformation($"LimiteCliente '{limiteClienteModel.Documento}' encontrado.");

            return LimiteClienteModel.ModelToEntity(limiteClienteModel);
        });
    }

    public async Task<IEnumerable<LimiteCliente>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await ExecuteWithExceptionHandling(async () =>
        {
            _appLogger.LogInformation("Buscando todos os LimitesClientes.");

            var search = _dynamoDbContext.ScanAsync<LimiteClienteModel>(new List<ScanCondition>());
            var result = await search.GetRemainingAsync(cancellationToken);

            _appLogger.LogInformation($"Encontrados {result.Count} LimitesClientes.");

            return result.ConvertAll(LimiteClienteModel.ModelToEntity);
        });
    }

    public async Task<bool> ExistsAsync(string documento, CancellationToken cancellationToken)
    {
        return await ExecuteWithExceptionHandling(async () =>
        {
            _appLogger.LogInformation($"Verificando se LimiteCliente '{documento}' existe.");

            var result = await _dynamoDbContext.LoadAsync<LimiteClienteModel>(documento, cancellationToken);
            if (result is null)
            {
                _appLogger.LogInformation($"LimiteCliente '{documento}' não encontrado.");
                return false;
            }

            _appLogger.LogInformation($"LimiteCliente '{documento}' encontrado.");
            return true;
        });
    }

    private async Task<LimiteClienteModel> GetModelByIdAsync(string id, CancellationToken cancellationToken)
    {
        _appLogger.LogInformation($"Buscando LimiteCliente '{id}'.");

        var limiteClienteModel = await _dynamoDbContext.LoadAsync<LimiteClienteModel>(id, cancellationToken);
        if (limiteClienteModel is null)
        {
            var message = $"LimiteCliente '{id}' não encontrado.";

            _appLogger.LogError(message);
            throw new NotFoundException(message);
        }

        _appLogger.LogInformation($"LimiteCliente '{limiteClienteModel.Documento}' encontrado.");

        return limiteClienteModel;
    }

    private static async Task<T> ExecuteWithExceptionHandling<T>(Func<Task<T>> func)
    {
        return await func();

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