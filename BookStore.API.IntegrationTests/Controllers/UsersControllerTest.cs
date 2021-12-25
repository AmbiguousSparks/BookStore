using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using BookStore.API.Common.Routes;
using BookStore.Application.Users.Commands.CreateUser;
using BookStore.Application.Users.Common.Models;
using BookStore.Domain.Common.Models;
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

        await AuthenticateAsync("test1@integration.com");

        //Act
        var users = await TestClient.GetAsync(ApiRoutes.Users.GetAll);

        //Assert
        users.StatusCode.Should().Be(HttpStatusCode.OK);
        (await users.Content.ReadFromJsonAsync<IEnumerable<UserOutDto>>()).Should().NotBeEmpty();
    }

    [Fact]
    public async Task GetPaged_ShouldReturnPagedUsers()
    {
        //Arrange
        var tasks = new List<Task>(3);
        tasks.Add(CreateTestUser("test1@gmail.com"));
        tasks.Add(CreateTestUser("test2@gmail.com"));
        tasks.Add(CreateTestUser("test3@gmail.com"));

        await Task.WhenAll(tasks);

        await AuthenticateAsync("test2@integration.com");

        //Act
        var users = await 
            TestClient.GetAsync($"{ApiRoutes.Users.GetPaged}?page=1&pageSize=2&sortColumn=firstName");

        //Assert
        users.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await users.Content.ReadFromJsonAsync<PaginationInfo<UserOutDto>>();
        response!.Total.Should().Be(4);
        response.PaginatedList.Should().HaveCount(2);
    }
    
    [Fact]
    public async Task Create_ShouldThrowExceptionWhenUserAlreadyExists()
    {
        //Arrange
        var tasks = new List<Task>(3);
        tasks.Add(CreateTestUser("test4@gmail.com"));

        await Task.WhenAll(tasks);

        await AuthenticateAsync("test3@integration.com");

        //Act
        var action = async () => await CreateTestUser("test4@gmail.com");
        
        //Assert
        await action.Should().ThrowAsync<HttpRequestException>();
    }

    private async Task CreateTestUser(string email)
    {
        var res = await TestClient.PostAsJsonAsync(ApiRoutes.Users.Create,
            new CreateUserCommand()
            {
                Email = email,
                Password = "S0M3P@SSword",
                ConfirmPassword = "S0M3P@SSword",
                FirstName = "Test",
                LastName = "Integration"
            });

        res.EnsureSuccessStatusCode();
    }
}