using FluentValidation;

namespace BookStore.Application.Users.Commands.Authenticate;

public class AuthenticateCommandValidator : AbstractValidator<AuthenticateCommand>
{
    public AuthenticateCommandValidator()
    {
        RuleFor(a => a.Email)
            .EmailAddress()
            .NotNull()
            .NotEmpty();

        RuleFor(a => a.Password)
            .NotEmpty()
            .Matches("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$");
    }
}