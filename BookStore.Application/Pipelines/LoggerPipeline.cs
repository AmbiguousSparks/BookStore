using BookStore.Application.Extensions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BookStore.Application.Pipelines;

public class LoggerPipeline<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<LoggerPipeline<TRequest, TResponse>> _logger;

    public LoggerPipeline(ILogger<LoggerPipeline<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
        RequestHandlerDelegate<TResponse> next)
    {
        _logger.RequestStartedLog(typeof(TRequest).Name, request);

        var response = await next();
        
        _logger.RequestFinishedLog(typeof(TRequest).Name, response);

        return response;
    }
}