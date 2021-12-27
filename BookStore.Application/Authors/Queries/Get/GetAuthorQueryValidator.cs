using FluentValidation;

namespace BookStore.Application.Authors.Queries.Get;

public class GetAuthorQueryValidator : AbstractValidator<GetAuthorQuery>
{
    public GetAuthorQueryValidator()
    {
        RuleFor(a => a.Id)
            .GreaterThan(0);
    }
}