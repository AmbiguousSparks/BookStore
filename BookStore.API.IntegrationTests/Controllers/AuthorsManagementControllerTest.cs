using System;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BookStore.API.Common.Routes;
using BookStore.Application.Authors.Commands.Create;
using Xunit;

namespace BookStore.API.IntegrationTests.Controllers;

public class AuthorsManagementControllerTest : IntegrationTest<AuthorsManagementControllerTest>
{
    [Fact]
    public async Task Create_ShouldCreateNewAuthor()
    {
        //Arrange
        await AuthenticateAsync();

        //Act
        var response = await TestClient.PostAsJsonAsync(ApiRoutes.AuthorsManagement.Create, new CreateAuthorCommand
        {
            Name = "Test",
            BirthDate = new DateTime(2000, 8, 23),
            Photo = "test.png"
        });

        //Assert
        response.EnsureSuccessStatusCode();
    }
}