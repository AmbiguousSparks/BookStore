using BookStore.Application.Profiles;
using BookStore.Domain.Models.Books;

namespace BookStore.Application.Authors.Common.Models;

public class AuthorDto : IMapFrom<Author>
{
    public string Name { get; set; } = default!;
    public string Photo { get; set; } = default!;
    public DateTime BirthDate { get; set; }
}