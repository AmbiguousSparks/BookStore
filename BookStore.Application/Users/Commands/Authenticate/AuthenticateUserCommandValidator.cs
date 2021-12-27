using FluentValidation;

namespace BookStore.Application.Users.Commands.Authenticate;

public class AuthenticateUserCommandValidator : AbstractValidator<AuthenticateUserCommand>
{
    public AuthenticateUserCommandValidator()
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