using MediatR;

namespace BookStore.Application.Cache;

[AttributeUsage(AttributeTargets.Class)]
public class CachedAttribute : Attribute
{
    public int CachedTime { get; }
    public string CacheGroup { get; set; }

    public CachedAttribute(int cachedTime, string cacheGroup)
    {
        CachedTime = cachedTime;
        CacheGroup = cacheGroup;
    }
}