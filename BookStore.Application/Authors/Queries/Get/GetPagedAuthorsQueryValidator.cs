using FluentValidation;

namespace BookStore.Application.Authors.Queries.Get;

public class GetPagedAuthorsQueryValidator : AbstractValidator<GetPagedAuthorsQuery>
{
    public GetPagedAuthorsQueryValidator()
    {
        RuleFor(a => a.Page)
            .GreaterThan(0);
        RuleFor(a => a.PageSize)
            .GreaterThan(0);
    }
}