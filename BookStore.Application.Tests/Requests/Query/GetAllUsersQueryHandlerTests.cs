using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BookStore.Application.Users.Common.Models;
using BookStore.Application.Users.Queries.GetUsers;
using BookStore.Domain.Common.Repositories.Interfaces;
using BookStore.Domain.Models.Users;
using NSubstitute;
using Xunit;

namespace BookStore.Application.Tests.Requests.Query;

public class GetAllUsersQueryHandlerTests
{
    private readonly IMapper _mapper = Substitute.For<IMapper>();
    private readonly IListRepository<User> _repository = Substitute.For<IListRepository<User>>();

    [Fact]
    public async Task Handle_ShouldReturnAllUsers()
    {
        //Arrange
        var handler = new GetAllUsersQuery.GetAllUsersQueryHandler(_mapper, _repository);

        //Act
        await handler.Handle(new GetAllUsersQuery(), CancellationToken.None);

        //Assert
        await _repository.ReceivedWithAnyArgs(1).GetAll();
        _mapper.ReceivedWithAnyArgs(1).Map<IEnumerable<UserOutDto>>(null);
    }
}