using BookStore.Application.Profiles;
using BookStore.Domain.Models.Users;

namespace BookStore.Application.Users.Common.Models;

public class UserInDto : IMapFrom<User>
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
}