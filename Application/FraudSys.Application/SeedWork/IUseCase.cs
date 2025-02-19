namespace FraudSys.Application.SeedWork;

public interface IUseCase<in TRequest, TResponse>
{
    Task<TResponse> Execute(TRequest request, CancellationToken cancellationToken);
}
