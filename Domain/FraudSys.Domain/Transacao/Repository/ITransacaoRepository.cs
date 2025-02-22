namespace FraudSys.Domain.Transacao.Repository;

public interface ITransacaoRepository : IRepository<TransacaoEntity, Guid>
{
    Task UpdateStatusAsync(TransacaoEntity transacao, CancellationToken cancellationToken);
}