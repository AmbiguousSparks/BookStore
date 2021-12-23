namespace BookStore.Domain.Common.Models;

public class UserToken
{
    public string AuthToken { get; set; } = default!;
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
}