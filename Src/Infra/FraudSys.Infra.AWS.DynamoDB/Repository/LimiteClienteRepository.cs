namespace FraudSys.Infra.AWS.DynamoDB.Repository;

public class LimiteClienteRepository : RepositoryBase<
        LimiteClienteRepository,
        LimiteClienteEntity,
        LimiteClienteModel,
        string>,
    ILimiteClienteRepository
{
    public LimiteClienteRepository(
        IAppLogger<LimiteClienteRepository> appLogger, IAmazonDynamoDB client, IUnitOfWork unitOfWork)
        : base(appLogger, client, unitOfWork, "LimiteClienteTable", "Documento")
    {
    }

    public async Task TransferirAsync(
        LimiteClienteEntity from,
        LimiteClienteEntity to,
        TransacaoEntity transacao,
        CancellationToken cancellationToken)
    {
        await ExecuteWithExceptionHandling(async () =>
        {
            var fromLimiteClienteModel = new LimiteClienteModel().EntityToModel(from);
            var toLimiteClienteModel = new LimiteClienteModel().EntityToModel(to);
            var transacaoModel = new TransacaoModel().EntityToModel(transacao);

            AppLogger.LogInformation($"Iniciando transferência de {transacaoModel.ValorTransferencia} de '{fromLimiteClienteModel.Documento}' para '{toLimiteClienteModel.Documento}'.");

            await UnitOfWork.AddTransaction(new TransactWriteItem
            {
                Update = new Update
                {
                    TableName = TableName,
                    Key = new Dictionary<string, AttributeValue>
                    {
                        { TableId, new AttributeValue { S = fromLimiteClienteModel.Documento } }
                    },
                    UpdateExpression = "SET LimiteTransacao = LimiteTransacao - :valorTransacao",
                    ConditionExpression = "LimiteTransacao >= :valorTransacao",
                    ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                    {
                        { ":valorTransacao", new AttributeValue { N = transacaoModel.ValorTransferencia.ToString(CultureInfo.InvariantCulture) } }
                    }
                }
            });

            await UnitOfWork.AddTransaction(new TransactWriteItem
            {
                Update = new Update
                {
                    TableName = TableName,
                    Key = new Dictionary<string, AttributeValue>
                    {
                        { TableId, new AttributeValue { S = toLimiteClienteModel.Documento } }
                    },
                    UpdateExpression = "SET LimiteTransacao = LimiteTransacao + :valorTransacao",
                    ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                    {
                        { ":valorTransacao", new AttributeValue { N = transacaoModel.ValorTransferencia.ToString(CultureInfo.InvariantCulture) } }
                    }
                }
            });

            AppLogger.LogInformation($"Transferência de {transacaoModel.ValorTransferencia} de '{fromLimiteClienteModel.Documento}' para '{toLimiteClienteModel.Documento}' adicionada as transações.");
        });
    }
}