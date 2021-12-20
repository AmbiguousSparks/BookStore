namespace BookStore.Domain.Common.Exceptions;

public interface IValidationError
{
    public string Error { get; }
}