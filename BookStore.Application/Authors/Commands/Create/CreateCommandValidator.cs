using FluentValidation;

namespace BookStore.Application.Authors.Commands.Create;

public class CreateCommandValidator : AbstractValidator<CreateCommand>
{
    public CreateCommandValidator()
    {
        RuleFor(a => a.Name)
            .NotEmpty();
        RuleFor(a => a.Photo)
            .NotEmpty();
    }
}