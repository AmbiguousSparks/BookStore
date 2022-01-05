namespace BookStore.Application.Cache.Config;

public class RedisConnection
{
    public string Host { get; set; } = default!;
    public int Port { get; set; }
}