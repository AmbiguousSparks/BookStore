using BookStore.Domain.Common.Models;
using BookStore.Domain.Common.Services;
using BookStore.Domain.Models.Users;
using MediatR;

namespace BookStore.Application.Users.Queries.Authentication;

internal class GetUserTokenQuery : IRequest<UserToken>
{
    public User User { get; set; } = default!;
    
    internal class GetUserTokenQueryHandler : IRequestHandler<GetUserTokenQuery, UserToken>
    {
        private readonly IAuthService _authService;

        public GetUserTokenQueryHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public Task<UserToken> Handle(GetUserTokenQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_authService.GenerateToken(request.User));
        }
    }
}