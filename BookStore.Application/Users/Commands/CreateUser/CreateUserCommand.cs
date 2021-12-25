using AutoMapper;
using BookStore.Application.Extensions;
using BookStore.Application.Users.Common.Models;
using BookStore.Application.Users.Events;
using BookStore.Application.Users.Queries.Authentication;
using BookStore.Domain.Common.Exceptions;
using BookStore.Domain.Common.Models;
using BookStore.Domain.Common.Repositories.Interfaces;
using BookStore.Domain.Models.Enums;
using BookStore.Domain.Models.Users;
using MediatR;
using OneOf;

namespace BookStore.Application.Users.Commands.CreateUser;

public class CreateUserCommand : UserInDto, IRequest<OneOf<UserToken, InvalidProperty, EntityAlreadyExists>>
{
    public string ConfirmPassword { get; set; } = default!;
    private UserType Type { get; set; }

    internal class CreateUserCommandHandler : IRequestHandler<CreateUserCommand,
        OneOf<UserToken, InvalidProperty, EntityAlreadyExists>>
    {
        private readonly IMapper _mapper;
        private readonly IRepository<User> _userRepository;
        private readonly IMediator _mediator;

        public CreateUserCommandHandler(IMapper mapper, IRepository<User> userRepository, IMediator mediator)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<OneOf<UserToken, InvalidProperty, EntityAlreadyExists>> Handle(CreateUserCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var userExists = await _userRepository.Exists(u => u.Email == request.Email, cancellationToken);

                if (userExists)
                    return new EntityAlreadyExists(nameof(User));

                var userModel = _mapper.Map<User>(request);

                userModel.Type = UserType.Admin;

                userModel.AddDomainEvent(new UserCreatedEvent());

                await _userRepository.Create(userModel, cancellationToken);

                await _mediator.DispatchDomainEvents(userModel);

                return await _mediator.Send(new GetUserTokenQuery {User = userModel}, cancellationToken);
            }
            catch (ArgumentNullException e)
            {
                return new InvalidProperty(nameof(User), e.Message);
            }
        }
    }
}