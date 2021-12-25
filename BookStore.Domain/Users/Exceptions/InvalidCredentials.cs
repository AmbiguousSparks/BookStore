using BookStore.Domain.Common.Exceptions;

namespace BookStore.Domain.Users.Exceptions;

public class InvalidCredentials : IValidationError
{
    public string Error => "Invalid Credentials. Try again.";
}