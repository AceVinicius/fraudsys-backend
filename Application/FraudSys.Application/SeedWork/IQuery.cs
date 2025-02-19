namespace FraudSys.Application.SeedWork;

public interface IQuery<in TRequest, TResponse> : IUseCase<TRequest, TResponse>
{

}
