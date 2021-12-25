using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BookStore.Domain.Common.Models;
using BookStore.Domain.Common.Services;
using BookStore.Domain.Models.Users;
using Microsoft.IdentityModel.Tokens;

namespace BookStore.Application.Authentication.Services;

internal sealed class AuthService : IAuthService
{
    private readonly TokenConfiguration _tokenConfiguration;
    private readonly TokenValidationParameters _tokenValidationParameters;    
    
    public AuthService(TokenConfiguration tokenConfiguration, TokenValidationParameters tokenValidationParameters)
    {
        _tokenConfiguration = tokenConfiguration ?? throw new ArgumentNullException(nameof(tokenConfiguration));
        _tokenValidationParameters = tokenValidationParameters ?? throw new ArgumentNullException(nameof(tokenValidationParameters));
    }

    public UserToken GenerateToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_tokenConfiguration.SecretKey);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new(ClaimTypes.Name, $"{user.FirstName}{user.LastName}"),
                new(ClaimTypes.Role, user.Type.ToString()),
                new(ClaimTypes.Email, user.Email)
            }),
            Expires = DateTime.Now.AddHours(_tokenConfiguration.ValidTime),
            NotBefore = DateTime.Now,
            SigningCredentials =
                new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return new UserToken
        {
            From = token.ValidFrom,
            To = token.ValidTo,
            AuthToken = tokenHandler.WriteToken(token)
        };
    }

    public string ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        tokenHandler.ValidateToken(token, _tokenValidationParameters, out var outToken);

        if (outToken is JwtSecurityToken jwtSecurityToken)
            return jwtSecurityToken.Claims.First(c => c.Type == ClaimTypes.Email).Value;
        return string.Empty;
    }
}