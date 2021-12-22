using System.Text;
using System.Text.Json;
using BookStore.Domain.Common.Services;
using Microsoft.Extensions.Caching.Distributed;

namespace BookStore.Application.Cache.Services;

public class CacheService : ICacheService
{
    private readonly IDistributedCache _distributedCache;

    public CacheService(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }

    public async Task SetAsync<T>(string cacheKey, T cacheValue, TimeSpan timeToLive,
        CancellationToken cancellationToken = default)
    {
        var serializedValue = JsonSerializer.SerializeToUtf8Bytes(cacheValue);

        await _distributedCache.SetAsync(cacheKey, serializedValue, new DistributedCacheEntryOptions
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

        return JsonSerializer.Deserialize<T>(serializedJson)!;
    }
}