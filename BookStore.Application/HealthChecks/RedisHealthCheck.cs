using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace BookStore.Application.HealthChecks;

public class RedisHealthCheck : IHealthCheck
{
    private readonly IDistributedCache _distributedCache;

    public RedisHealthCheck(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache ?? throw new ArgumentNullException(nameof(distributedCache));
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
        CancellationToken cancellationToken = new CancellationToken())
    {
        try
        {
            await _distributedCache.GetAsync(nameof(RedisHealthCheck), cancellationToken);
            return HealthCheckResult.Healthy("Connected");
        }
        catch (Exception)
        {
            return HealthCheckResult.Unhealthy("It was not possible to connect to redis server");
        }
    }
}