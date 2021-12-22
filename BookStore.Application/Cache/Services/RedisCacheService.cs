using System.Text;
using BookStore.Domain.Common.Services;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace BookStore.Application.Cache.Services;

internal class RedisCacheService : ICacheService
{
    private readonly IDistributedCache _distributedCache;

    public RedisCacheService(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }

    public async Task SetAsync<T>(string cacheKey, T cacheValue, TimeSpan timeToLive,
        CancellationToken cancellationToken = default)
    {
        var serializedValue = JsonConvert.SerializeObject(cacheValue);

        await _distributedCache.SetAsync(cacheKey, Encoding.UTF8.GetBytes(serializedValue),
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = timeToLive
            }, cancellationToken);
    }

    public async Task<T> GetAsync<T>(string cacheKey, CancellationToken cancellationToken = default)
    {
        var serializedValue = await _distributedCache.GetAsync(cacheKey, cancellationToken);

        if (serializedValue is null)
            return default!;

        var serializedJson = Encoding.UTF8.GetString(serializedValue);

        return JsonConvert.DeserializeObject<T>(serializedJson);
    }
}