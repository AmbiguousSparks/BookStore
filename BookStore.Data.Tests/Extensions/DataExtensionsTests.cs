using BookStore.Application.Common.Interfaces;
using BookStore.Data.Extensions;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Xunit;

namespace BookStore.Data.Tests.Extensions;

public class DataExtensionsTests
{
    private readonly IConfiguration _configuration = Substitute.For<IConfiguration>();
    private readonly IServiceCollection _services = new ServiceCollection();
    
    [Fact]
    public void AddData_ShouldAddDbContext()
    {
        //Arrange

        var configurationSection = Substitute.For<IConfigurationSection>();

        configurationSection[Arg.Any<string>()].ReturnsForAnyArgs("test connection");
        
        _configuration
            .GetSection("ConnectionStrings").Returns(configurationSection);

        //Act
        _services.AddData(_configuration);

        //Assert
        var dbContext = _services.BuildServiceProvider().GetService<IBookStoreDbContext>();
        dbContext.Should().NotBeNull();
    }
}