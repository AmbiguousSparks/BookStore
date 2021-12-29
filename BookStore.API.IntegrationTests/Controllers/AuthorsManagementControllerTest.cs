using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using BookStore.API.Common.Routes;
using BookStore.Application.Authors.Commands.Create;
using BookStore.Application.Authors.Common.Models;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
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
    
    [Fact]
    public async Task GetAll_ShouldReturnAllAuthors()
    {
        //Arrange
        CacheService.GetAsync<IEnumerable<AuthorDto>>(Arg.Any<string>(),
                Arg.Any<CancellationToken>())
            .ReturnsNullForAnyArgs();
        await AuthenticateAsync();
        await TestClient.PostAsJsonAsync(ApiRoutes.AuthorsManagement.Create, new CreateAuthorCommand
        {
            Name = "Test",
            BirthDate = new DateTime(2000, 8, 23),
            Photo = "test.png"
        });

        //Act
        var response = await TestClient.GetAsync(ApiRoutes.AuthorsManagement.GetAll);

        //Assert
        response.EnsureSuccessStatusCode();
        var authors = await response.Content.ReadFromJsonAsync<IEnumerable<AuthorDto>>();
        authors.Should().HaveCount(1);
    }
}