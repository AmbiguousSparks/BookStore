using BookStore.Application.Users.Queries.Authentication;
using BookStore.Domain.Common.Models;
using BookStore.Domain.Common.Repositories.Interfaces;
using BookStore.Domain.Models.Users;
using BookStore.Domain.Users.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using OneOf;

namespace BookStore.Application.Users.Commands.Authenticate;

public class AuthenticateCommand : IRequest<OneOf<UserToken, InvalidCredentials>>
{
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;

    internal class AuthenticateCommandHandler : IRequestHandler<AuthenticateCommand,
        OneOf<UserToken, InvalidCredentials>>
    {
        private readonly IMediator _mediator;
        private readonly IRepository<User> _userRepository;
        private readonly IPasswordHasher<User> _hasher;

        public AuthenticateCommandHandler(IMediator mediator, IRepository<User> userRepository,
            IPasswordHasher<User> hasher)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _hasher = hasher ?? throw new ArgumentNullException(nameof(hasher));
        }

        public async Task<OneOf<UserToken, InvalidCredentials>>
            Handle(AuthenticateCommand request, CancellationToken cancellationToken)
        {
            if (!await _userRepository.Exists(u => u.Email == request.Email, cancellationToken))
                return new InvalidCredentials();

            var user = await _userRepository.Get(u => u.Email == request.Email,
                cancellationToken);

            if (_hasher.VerifyHashedPassword(user, user.Password, request.Password) ==
                PasswordVerificationResult.Failed)
                return new InvalidCredentials();

            return await _mediator.Send(new GetUserTokenQuery {User = user}, cancellationToken);
        }
    }
}