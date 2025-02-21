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
        var limiteClienteModel = LimiteClienteModel.EntityToModel(input);

        await _dynamoDbContext.SaveAsync(limiteClienteModel, cancellationToken);

        return input;
    }

    public async Task<LimiteCliente> UpdateAsync(LimiteCliente input, CancellationToken cancellationToken)
    {
        var limiteClienteModel = LimiteClienteModel.EntityToModel(input);

        await _dynamoDbContext.SaveAsync(limiteClienteModel, cancellationToken);

        return input;
    }

    public async Task<LimiteCliente?> DeleteAsync(string id, CancellationToken cancellationToken)
    {
        var limiteClienteModel = await _dynamoDbContext.LoadAsync<LimiteClienteModel>(
            id,
            cancellationToken);
        
        if (limiteClienteModel == null)
        {
            return null;
        }

        await _dynamoDbContext.DeleteAsync(limiteClienteModel, cancellationToken);

        return LimiteClienteModel.ModelToEntity(limiteClienteModel);
    }

    public async Task<LimiteCliente?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        var limiteClienteModel = await _dynamoDbContext.LoadAsync<LimiteClienteModel>(id, cancellationToken);

        if (limiteClienteModel == null)
        {
            return null;
        }

        return LimiteClienteModel.ModelToEntity(limiteClienteModel);
    }

    public async Task<IEnumerable<LimiteCliente>> GetAllAsync(CancellationToken cancellationToken)
    {
        var search = _dynamoDbContext.ScanAsync<LimiteClienteModel>(new List<ScanCondition>());

        var result = await search.GetRemainingAsync();

        return result.ConvertAll(LimiteClienteModel.ModelToEntity);
    }
}