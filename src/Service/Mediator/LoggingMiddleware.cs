using MediatR;

namespace Service.Mediator;

public class LoggingMiddleware<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<TRequest> _logger;

    public LoggingMiddleware(ILogger<TRequest> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Mediatr Request {InputType} {@InputParameters}", typeof(TRequest).Name, request);

        TResponse? response;
        try
        {
            response = await next();
        }
        catch (Exception e)
        {
            _logger.LogError("Mediatr Error {@Exception} ", e);
            throw;
        }

        _logger.LogInformation("Mediatr Response {OutputType}  {@Output}", typeof(TResponse).Name, response);

        return response;
    }
}