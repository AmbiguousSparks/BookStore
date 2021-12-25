using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BookStore.API.Common.Routes;
using BookStore.Application.Common.Interfaces;
using BookStore.Application.Users.Commands.CreateUser;
using BookStore.Data.Context;
using BookStore.Domain.Common.Models;
using BookStore.Domain.Common.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NSubstitute;

namespace BookStore.API.IntegrationTests;

public class IntegrationTest
{
    protected HttpClient TestClient { get; }
    protected ICacheService CacheService { get; }

    protected IntegrationTest()
    {
        CacheService = Substitute.For<ICacheService>();

        var factory = new WebApplicationFactory<Startup>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.RemoveAll(typeof(IBookStoreDbContext));
                    services.RemoveAll(typeof(BookStoreDbContext));
                    services.RemoveAll(typeof(DbContextOptions<BookStoreDbContext>));
                    services.RemoveAll(typeof(ICacheService));
                    services.AddScoped(_ => CacheService);
                    services.AddDbContext<BookStoreDbContext>(c =>
                        c.UseInMemoryDatabase("TestIntegration"));
                    services
                        .AddScoped<IBookStoreDbContext>(provider => provider.GetRequiredService<BookStoreDbContext>());
                });
            });
        TestClient = factory.CreateClient();
    }

    protected async Task AuthenticateAsync()
    {
        TestClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", await GetJwtAsync());
    }

    private async Task<string> GetJwtAsync()
    {
        var response = await TestClient.PostAsJsonAsync(ApiRoutes.Users.Create,
            new CreateUserCommand
            {
                Email = "test@integration.com",
                Password = "S0M3P@SSword",
                ConfirmPassword = "S0M3P@SSword",
                FirstName = "Test",
                LastName = "Integration"
            });

        response.EnsureSuccessStatusCode();

        var token = await response.Content.ReadFromJsonAsync<UserToken>();
        return token!.AuthToken;
    }
}