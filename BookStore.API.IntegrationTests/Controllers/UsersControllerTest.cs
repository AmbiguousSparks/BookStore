using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using BookStore.API.Common.Routes;
using BookStore.Application.Users.Commands.Authenticate;
using BookStore.Application.Users.Commands.Create;
using BookStore.Application.Users.Common.Models;
using BookStore.Domain.Common.Models;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Xunit;

namespace BookStore.API.IntegrationTests.Controllers;

public class UsersControllerTest : IntegrationTest<UsersControllerTest>
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

    [Fact]
    public async Task GetPaged_ShouldReturnPagedUsers()
    {
        //Arrange
        var tasks = new List<Task>(3);
        tasks.Add(CreateTestUser("test1@gmail.com"));
        tasks.Add(CreateTestUser("test2@gmail.com"));
        tasks.Add(CreateTestUser("test3@gmail.com"));

        await Task.WhenAll(tasks);

        await AuthenticateAsync();

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
        tasks.Add(CreateTestUser("test@gmail.com"));

        await Task.WhenAll(tasks);

        await AuthenticateAsync();
        
        //Act
        var action = async () => await CreateTestUser("test@gmail.com");
        
        //Assert
        await action.Should().ThrowAsync<HttpRequestException>();
    }
    
    [Fact]
    public async Task? Login_ShouldReturnUserTokenWhenValidCredentials()
    {
        //Arrange
        var tasks = new List<Task>(3);
        tasks.Add(CreateTestUser("test@gmail.com"));

        await Task.WhenAll(tasks);
        
        //Act
        var response = await TestClient.PostAsJsonAsync(ApiRoutes.Users.Authenticate, new AuthenticateUserCommand
        {
            Email = "test@gmail.com",
            Password = "S0M3P@SSword"
        });
        
        //Assert
        var authToken = await response.Content.ReadFromJsonAsync<UserToken>();
        authToken.Should().NotBeNull();
        authToken!.AuthToken.Should().NotBeEmpty();
    }
    
    [Fact]
    public async Task? Login_ShouldReturnInvalidCredentialsException()
    {
        //Arrange
       
        //Act
        var response = await TestClient.PostAsJsonAsync(ApiRoutes.Users.Authenticate, new AuthenticateUserCommand
        {
            Email = "test@gmail.com",
            Password = "S0M3P@SSword1213213"
        });

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var message = await response.Content.ReadAsStringAsync();
        message.Should().Contain("Invalid Credentials");
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