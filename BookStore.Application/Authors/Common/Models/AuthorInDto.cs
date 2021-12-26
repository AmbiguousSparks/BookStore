using BookStore.Application.Profiles;
using BookStore.Domain.Models.Books;

namespace BookStore.Application.Authors.Common.Models;

public class AuthorInDto : IMapFrom<Author>
{
    public string Name { get; set; } = default!;
    public string Photo { get; set; } = default!;
    public DateOnly BirthDate { get; set; }
}