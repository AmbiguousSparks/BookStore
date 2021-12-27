using FluentValidation;

namespace BookStore.Application.Authors.Commands.Update;

public class UpdateAuthorCommandValidator : AbstractValidator<UpdateAuthorCommand>
{
    public UpdateAuthorCommandValidator()
    {
        RuleFor(a => a.Name)
            .NotEmpty();
        RuleFor(a => a.Photo)
            .NotNull();
        RuleFor(a => a.Id)
            .GreaterThan(0);
    }
}