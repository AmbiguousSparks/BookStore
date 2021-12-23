namespace BookStore.Domain.Common.Models;

public class TokenConfiguration
{
    public string SecretKey { get; set; } = default!;
    public int ValidTime { get; set; }
}