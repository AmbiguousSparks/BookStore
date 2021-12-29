using BookStore.Application.Cache;
using BookStore.Domain.Common.Services;
using MediatR;

namespace BookStore.Application.Pipelines;

public class CachePipeline<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ICacheService _cacheService;

    public CachePipeline(ICacheService cacheService)
    {
        _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
        RequestHandlerDelegate<TResponse> next)
    {
        if (Attribute.GetCustomAttribute(typeof(TRequest), typeof(CachedAttribute)) is not CachedAttribute
            cacheAttribute)
            return await next();

        try
        {
            var cachedValue = await _cacheService.GetAsync<TResponse>(cacheAttribute.Key, cancellationToken);
            if (cachedValue is not null)
                return cachedValue;

            var response = await next();
            await _cacheService.SetAsync(cacheAttribute.Key, response, TimeSpan.FromSeconds(cacheAttribute.CachedTime),
                cancellationToken);
            return response;
        }
        catch (Exception)
        {
            return await next();
        }
    }
}