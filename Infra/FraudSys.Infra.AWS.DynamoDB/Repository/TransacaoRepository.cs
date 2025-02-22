namespace FraudSys.Infra.AWS.DynamoDB.Repository;

public class TransacaoRepository : RepositoryBase<
        TransacaoRepository,
        TransacaoEntity,
        TransacaoModel,
        Guid>,
    ITransacaoRepository
{
    public TransacaoRepository(
        IAppLogger<TransacaoRepository> appLogger, IAmazonDynamoDB client, IUnitOfWork unitOfWork)
        : base(appLogger, client, unitOfWork, "TransacaoTable", "Id")
    {
    }

    public Task UpdateStatusAsync(TransacaoEntity transacao, CancellationToken cancellationToken)
    {
        var request = new UpdateItemRequest
        {
            TableName = TableName,
            Key = new Dictionary<string, AttributeValue>
            {
                { TableId, new AttributeValue { S = transacao.Id.ToString() } }
            },
            UpdateExpression = "SET #Status = :status",
            ExpressionAttributeNames = new Dictionary<string, string>
            {
                { "#Status", "Status" }
            },
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                { ":status", new AttributeValue { N = transacao.Status.ToString() } }
            }
        };

        return Client.UpdateItemAsync(request, cancellationToken);
    }
}