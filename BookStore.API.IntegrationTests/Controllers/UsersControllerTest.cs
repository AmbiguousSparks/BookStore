using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using BookStore.API.Common.Routes;
using BookStore.Application.Users.Common.Models;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Xunit;

namespace BookStore.API.IntegrationTests.Controllers;

public class UsersControllerTest : IntegrationTest
{
    [Fact]
    public async Task GetAll_ShouldReturnAllUsers()
    {
        //Arrange
        CacheService.GetAsync<IEnumerable<UserOutDto>>(Arg.Any<string>(), 
                Arg.Any<CancellationToken>())
            .ReturnsNullForAnyArgs();

        await AuthenticateAsync();

        //Act
        var users = await TestClient.GetAsync(ApiRoutes.Users.GetAll);

        //Assert
        users.StatusCode.Should().Be(HttpStatusCode.OK);
        (await users.Content.ReadFromJsonAsync<IEnumerable<UserOutDto>>()).Should().NotBeEmpty();
    }
}