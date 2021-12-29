using System.Text;
using BookStore.Application.Cache.Config;
using BookStore.Domain.Common.Services;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace BookStore.Application.Cache.Services;

internal class RedisCacheService : ICacheService
{
    private readonly IDistributedCache _distributedCache;
    private readonly RedisConnection _redisConnection;

    public RedisCacheService(IDistributedCache distributedCache, RedisConnection redisConnection)
    {
        _distributedCache = distributedCache;
        _redisConnection = redisConnection;
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

    public IEnumerable<string> GetKeys(string pattern)
    {
        try
        {
            var connection = ConnectionMultiplexer.Connect($"{_redisConnection.Host}:{_redisConnection.Port}");
            var server = connection.GetServer(_redisConnection.Host, _redisConnection.Port);
            var keys = server.Keys(pattern: pattern);
            return keys.Select(k => k.ToString());
        }
        catch (Exception)
        {
            return Enumerable.Empty<string>();
        }
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        await _distributedCache.RemoveAsync(key, cancellationToken);
    }
}