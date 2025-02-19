namespace FraudSys.Application.SeedWork;

public interface ICommand<in TRequest, TResponse> : IUseCase<TRequest, TResponse>
{

}
