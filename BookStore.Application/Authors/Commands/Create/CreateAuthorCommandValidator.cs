using FluentValidation;

namespace BookStore.Application.Authors.Commands.Create;

public class CreateAuthorCommandValidator : AbstractValidator<CreateAuthorCommand>
{
    public CreateAuthorCommandValidator()
    {
        RuleFor(a => a.Name)
            .NotEmpty();
        RuleFor(a => a.Photo)
            .NotEmpty();
    }
}