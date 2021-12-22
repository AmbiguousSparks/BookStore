using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BookStore.Application.Cache.Services;
using BookStore.Domain.Models.Enums;
using BookStore.Domain.Models.Users;
using FluentAssertions;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using NSubstitute;
using Xunit;

namespace BookStore.Application.Tests.Cache;

public class CacheServiceTests
{
    private readonly IDistributedCache _distributedCache = Substitute.For<IDistributedCache>();

    [Fact]
    public async Task SetAsync_ShouldSetANewValueToAKey()
    {
        //Arrange
        const string cacheValue = "this is a cache value";
        const string cacheKey = "ThisIsAKey";
        var cacheService = new RedisCacheService(_distributedCache);

        //Act
        await cacheService.SetAsync(cacheKey, cacheValue, TimeSpan.FromHours(1));

        //Assert
        await _distributedCache.Received(1).SetAsync(cacheKey, Arg.Any<byte[]>(),
            Arg.Any<DistributedCacheEntryOptions>(),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetAsync_ShouldReturnValueFromKey()
    {
        //Arrange
        var user = new User("Daniel", "Santos", "daniel@gmail.com", "Teste@1234", UserType.Default);

        var jsonUser = JsonConvert.SerializeObject(user);

        const string cacheKey = "ThisIsAKey";
        var cacheService = new RedisCacheService(_distributedCache);
        _distributedCache.GetAsync(Arg.Any<string>()).ReturnsForAnyArgs(Encoding.UTF8.GetBytes(jsonUser));

        //Act
        var result = await cacheService.GetAsync<User>(cacheKey);

        //Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(user);
    }
}