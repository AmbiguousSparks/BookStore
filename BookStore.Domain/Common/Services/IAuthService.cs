using BookStore.Domain.Common.Models;
using BookStore.Domain.Models.Users;

namespace BookStore.Domain.Common.Services;

public interface IAuthService
{
    public UserToken GenerateToken(User user);
    
    public string ValidateToken(string token);
}