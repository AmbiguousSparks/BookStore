using BookStore.Application.Cache;
using BookStore.Domain.Common.Services;
using MediatR;
using Newtonsoft.Json;

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
            var key = GetKey(request, cacheAttribute.CacheGroup);
            
            var cachedValue = await _cacheService.GetAsync<TResponse>(key, cancellationToken);
            if (cachedValue is not null)
                return cachedValue;
            
            var response = await next();
            await _cacheService.SetAsync(key, response, TimeSpan.FromSeconds(cacheAttribute.CachedTime),
                cancellationToken);
            return response;
        }
        catch (Exception)
        {
            return await next();
        }
    }

    private static string GetKey(TRequest request, string cacheGroup)
    {
        var jsonKey = JsonConvert.SerializeObject(request);
        return $"{cacheGroup} | {typeof(TRequest).Name} | {jsonKey}";
    }
}