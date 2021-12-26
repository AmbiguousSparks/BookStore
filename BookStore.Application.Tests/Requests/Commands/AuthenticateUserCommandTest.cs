using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using BookStore.Application.Users.Commands.Authenticate;
using BookStore.Application.Users.Queries.Authentication;
using BookStore.Domain.Common.Models;
using BookStore.Domain.Common.Repositories.Interfaces;
using BookStore.Domain.Models.Enums;
using BookStore.Domain.Models.Users;
using BookStore.Domain.Users.Exceptions;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using Xunit;

namespace BookStore.Application.Tests.Requests.Commands;

public class AuthenticateUserCommandTest
{
    private readonly IMediator _mediator = Substitute.For<IMediator>();
    private readonly IRepository<User> _repository = Substitute.For<IRepository<User>>();
    private readonly IPasswordHasher<User> _hasher = Substitute.For<IPasswordHasher<User>>();

    [Fact]
    public async Task Handle_ShouldReturnUserTokenWhenValidCredentials()
    {
        //Arrange
        _repository.Exists(Arg.Any<Expression<Func<User, bool>>>()).ReturnsForAnyArgs(true);
        _repository.Get(Arg.Any<Expression<Func<User, bool>>>())
            .ReturnsForAnyArgs(new User("Unit", "Test", 
                "test@gmail.com", "JUST4T3ST", UserType.Default));
        _mediator.Send(Arg.Any<GetUserTokenQuery>(), Arg.Any<CancellationToken>())
            .ReturnsForAnyArgs(new UserToken());
        _hasher.VerifyHashedPassword(Arg.Any<User>(), Arg.Any<string>(), Arg.Any<string>())
            .ReturnsForAnyArgs(PasswordVerificationResult.Success);

        var handler = new AuthenticateCommand.AuthenticateCommandHandler(_mediator, _repository, _hasher);

        //Act
        var response = await handler.Handle(new AuthenticateCommand
        {
            Email = "test@gmail.com",
            Password = "JUST4T3ST"
        }, CancellationToken.None);

        //Assert
        response.Value.Should().BeAssignableTo<UserToken>();
        response.AsT0.AuthToken.Should().NotBeEmpty();
    }
    
    [Fact]
    public async Task Handle_ShouldReturnInvalidCredentialsException()
    {
        //Arrange
        _repository.Exists(Arg.Any<Expression<Func<User, bool>>>()).ReturnsForAnyArgs(false);
        
        var handler = new AuthenticateCommand.AuthenticateCommandHandler(_mediator, _repository, _hasher);

        //Act
        var response = await handler.Handle(new AuthenticateCommand
        {
            Email = "test@gmail.com",
            Password = "JUST4T3ST"
        }, CancellationToken.None);

        //Assert
        response.Value.Should().BeAssignableTo<InvalidCredentials>();
        response.AsT1.Error.Should().Be("Invalid Credentials. Try again.");
    }
    
    [Fact]
    public async Task Handle_ShouldReturnInvalidCredentialsExceptionWhenPasswordIsNotValid()
    {
        //Arrange
        _repository.Exists(Arg.Any<Expression<Func<User, bool>>>()).ReturnsForAnyArgs(true);
        _repository.Get(Arg.Any<Expression<Func<User, bool>>>())
            .ReturnsForAnyArgs(new User("Unit", "Test", 
                "test@gmail.com", "JUST4T3ST", UserType.Default));
        _hasher.VerifyHashedPassword(Arg.Any<User>(), Arg.Any<string>(), Arg.Any<string>())
            .ReturnsForAnyArgs(PasswordVerificationResult.Failed);
        
        var handler = new AuthenticateCommand.AuthenticateCommandHandler(_mediator, _repository, _hasher);

        //Act
        var response = await handler.Handle(new AuthenticateCommand
        {
            Email = "test@gmail.com",
            Password = "JUST4T3ST"
        }, CancellationToken.None);

        //Assert
        response.Value.Should().BeAssignableTo<InvalidCredentials>();
        response.AsT1.Error.Should().Be("Invalid Credentials. Try again.");
    }
}