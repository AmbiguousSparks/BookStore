using FluentValidation;

namespace BookStore.Application.Users.Commands.CreateUser;

public class CreateUserRequestValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserRequestValidator()
    {
        RuleFor(u => u.FirstName)
            .NotEmpty();
        RuleFor(u => u.LastName)
            .NotEmpty();
        RuleFor(u => u.Password)
            .Matches("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$")
            .Equal(u => u.ConfirmPassword);
        RuleFor(u => u.Email)
            .EmailAddress();
    }
}