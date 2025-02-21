namespace FraudSys.Service.API.Filter;

public class ApiGlobalExceptionFilter : IExceptionFilter
{
    private readonly IHostEnvironment _env;
    private readonly IAppLogger<ApiGlobalExceptionFilter> _appLogger;

    public ApiGlobalExceptionFilter(
        IHostEnvironment env,
        IAppLogger<ApiGlobalExceptionFilter> appLogger)
    {
        _env = env;
        _appLogger = appLogger;
    }

    public void OnException(ExceptionContext context)
    {
        var problemDetails = new ProblemDetails();
        bool isExceptionHandled;

        _appLogger.LogInformation($"Handling exception: {context.Exception.Message}");

        if (_env.IsDevelopment())
        {
            _appLogger.LogTrace($"Stack Trace: {context.Exception.StackTrace ?? string.Empty}");
            problemDetails.Extensions.Add("StackTrace", context.Exception.StackTrace);
        }

        switch (context.Exception)
        {
            case DatabaseException:
                problemDetails.Title = nameof(DatabaseException) + ": An error occurred";
                problemDetails.Status = StatusCodes.Status503ServiceUnavailable;
                problemDetails.Type = nameof(DatabaseException);
                problemDetails.Detail = context.Exception.Message;

                isExceptionHandled = true;
                break;

            case NotFoundException:
                problemDetails.Title = nameof(NotFoundException) + ": An error occurred";
                problemDetails.Status = StatusCodes.Status404NotFound;
                problemDetails.Type = nameof(NotFoundException);
                problemDetails.Detail = context.Exception.Message;

                isExceptionHandled = true;
                break;

            case FoundException:
                problemDetails.Title = nameof(FoundException) + ": An error occurred";
                problemDetails.Status = StatusCodes.Status400BadRequest;
                problemDetails.Type = nameof(FoundException);
                problemDetails.Detail = context.Exception.Message;

                isExceptionHandled = true;
                break;

            case EntityValidationException:
                problemDetails.Title = nameof(EntityValidationException) + ": An error occurred";
                problemDetails.Status = StatusCodes.Status400BadRequest;
                problemDetails.Type = nameof(EntityValidationException);
                problemDetails.Detail = context.Exception.Message;

                isExceptionHandled = true;
                break;

            case EntityCreationException:
                problemDetails.Title = nameof(EntityCreationException) + ": An error occurred";
                problemDetails.Status = StatusCodes.Status400BadRequest;
                problemDetails.Type = nameof(EntityCreationException);
                problemDetails.Detail = context.Exception.Message;

                isExceptionHandled = true;
                break;

            case OperationCanceledException:
                problemDetails.Title = nameof(OperationCanceledException) + ": An error occurred";
                problemDetails.Status = StatusCodes.Status408RequestTimeout;
                problemDetails.Type = nameof(OperationCanceledException);
                problemDetails.Detail = context.Exception.Message;

                isExceptionHandled = true;
                break;

            default:
                _appLogger.LogCritical(context.Exception.Message);
                _appLogger.LogCritical(context.Exception.StackTrace ?? string.Empty);

                problemDetails.Title = "An error occurred";
                problemDetails.Status = StatusCodes.Status500InternalServerError;
                problemDetails.Type = nameof(Exception);
                problemDetails.Detail = context.Exception.Message;

                isExceptionHandled = false;
                break;
        }

        context.HttpContext.Response.StatusCode = (int) problemDetails.Status;
        context.ExceptionHandled = isExceptionHandled;
        context.Result = new ObjectResult(problemDetails);
    }
}