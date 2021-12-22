using MediatR;

namespace BookStore.Application.Cache;

[AttributeUsage(AttributeTargets.Class)]
public class CachedAttribute : Attribute
{
    public string Key { get; }
    public int CachedTime { get; }

    public CachedAttribute(int cachedTime, string key)
    {
        CachedTime = cachedTime;
        Key = key;
    }
}