namespace BookStore.API.Common.Models.Config;

public class CorsConfig
{
    public IEnumerable<string> CorsDomains { get; set; } = Enumerable.Empty<string>();
}