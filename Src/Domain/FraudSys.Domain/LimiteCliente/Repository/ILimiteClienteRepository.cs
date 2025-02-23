using FraudSys.Domain.Transacao;

namespace FraudSys.Domain.LimiteCliente.Repository;

public interface ILimiteClienteRepository : IRepository<LimiteClienteEntity, string>
{
    Task TransferirAsync(
        LimiteClienteEntity from,
        LimiteClienteEntity to,
        TransacaoEntity transacao,
        CancellationToken cancellationToken);
}