using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BookStore.Application.Extensions;
using BookStore.Application.Users.Commands.Create;
using BookStore.Application.Users.Queries.Authentication;
using BookStore.Domain.Common.Exceptions;
using BookStore.Domain.Common.Models;
using BookStore.Domain.Common.Repositories.Interfaces;
using BookStore.Domain.Models.Enums;
using BookStore.Domain.Models.Users;
using FluentAssertions;
using MediatR;
using NSubstitute;
using Xunit;

namespace BookStore.Application.Tests.Requests.Commands;

public class CreateUserCommandHandlerTest
{
    private readonly IMediator _mediator = Substitute.For<IMediator>();
    private readonly IRepository<User> _repository = Substitute.For<IRepository<User>>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();

    [Fact]
    public async Task Handle_ShouldCreateNewUser()
    {
        //Arrange
        _repository.Exists(Arg.Any<Expression<Func<User, bool>>>()).Returns(false);
        var user = new User("Daniel",
            "Santos", "daniel@gmail.com", "Teste@password123", UserType.Default);
        _mapper.Map<User>(Arg.Any<CreateCommand>()).Returns(user);
        _mediator.Send(Arg.Any<GetUserTokenQuery>(), Arg.Any<CancellationToken>())
            .ReturnsForAnyArgs(new UserToken());

        var handler = new CreateCommand.CreateUserCommandHandler(_mapper, _repository, _mediator);

        //Act
        var response = await handler.Handle(new CreateCommand(), CancellationToken.None);

        //Assert
        _mapper.ReceivedWithAnyArgs(1).Map<User>(Arg.Any<CreateCommand>());
        await _repository.ReceivedWithAnyArgs(1).Exists(Arg.Any<Expression<Func<User, bool>>>());
        await _repository.ReceivedWithAnyArgs(1).Create(Arg.Any<User>());
        await _mediator.ReceivedWithAnyArgs(1).DispatchDomainEvents(user);
        response.Value.Should().BeAssignableTo<UserToken>();
    }

    [Fact]
    public async Task Handle_ShouldThrowExceptionWhenUserAlreadyExists()
    {
        //Arrange
        _repository.Exists(Arg.Any<Expression<Func<User, bool>>>()).Returns(true);
        var user = new User("Daniel",
            "Santos", "daniel@gmail.com", "Teste@password123", UserType.Default);
        _mapper.Map<User>(Arg.Any<CreateCommand>()).Returns(user);

        var handler =
            new CreateCommand.CreateUserCommandHandler(_mapper, _repository, _mediator);

        //Act
        var response = await handler.Handle(new CreateCommand(), CancellationToken.None);


        //Assert
        _mapper.DidNotReceiveWithAnyArgs().Map<User>(Arg.Any<CreateCommand>());
        await _repository.ReceivedWithAnyArgs(1).Exists(Arg.Any<Expression<Func<User, bool>>>());
        await _repository.DidNotReceiveWithAnyArgs().Create(Arg.Any<User>());
        await _mediator.DidNotReceiveWithAnyArgs().DispatchDomainEvents(user);
        response.Value.Should().BeAssignableTo<EntityAlreadyExists>();
    }
}