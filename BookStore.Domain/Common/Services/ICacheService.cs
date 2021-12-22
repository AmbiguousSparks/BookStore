namespace BookStore.Domain.Common.Services;

public interface ICacheService
{
    Task SetAsync<T>(string cacheKey, T cacheValue, TimeSpan timeToLive, CancellationToken cancellationToken = default);
    Task<T> GetAsync<T>(string cacheKey, CancellationToken cancellationToken = default);
}