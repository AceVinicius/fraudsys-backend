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
}