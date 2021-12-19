namespace BookStore.Application.Common.Models;

public class PropertyError
{
    public string PropertyName { get; set; } = default!;
    public string ErrorMessage { get; set; } = default!;
}