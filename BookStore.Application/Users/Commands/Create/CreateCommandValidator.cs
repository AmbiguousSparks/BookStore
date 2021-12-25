using FluentValidation;

namespace BookStore.Application.Users.Commands.Create;

public class CreateCommandValidator : AbstractValidator<CreateCommand>
{
    public CreateCommandValidator()
    {
        RuleFor(u => u.FirstName)
            .NotEmpty();
        RuleFor(u => u.LastName)
            .NotEmpty();
        RuleFor(u => u.Password)
            .NotEmpty()
            .Matches("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$")
            .Must((dto, pass) => pass == dto.ConfirmPassword);
        RuleFor(u => u.ConfirmPassword)
            .NotEmpty();
        RuleFor(u => u.Email)
            .EmailAddress();
    }
}