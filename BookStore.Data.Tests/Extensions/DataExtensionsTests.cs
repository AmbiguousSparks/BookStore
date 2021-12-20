using System;
using BookStore.Application.Common.Interfaces;
using BookStore.Data.Extensions;
using BookStore.Domain.Common.Repositories.Interfaces;
using BookStore.Domain.Models.Users;
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
    public void AddDbContext_ShouldAddDbContext()
    {
        //Arrange

        var configurationSection = Substitute.For<IConfigurationSection>();

        configurationSection[Arg.Any<string>()].ReturnsForAnyArgs("Data Source=MyDb.db");
        
        _configuration
            .GetSection("ConnectionStrings").Returns(configurationSection);

        //Act
        _services.AddDbContext(_configuration);

        //Assert
        var dbContext = _services.BuildServiceProvider().GetService<IBookStoreDbContext>();
        dbContext.Should().NotBeNull();
    }
    
    //Arrange
    [Theory]
    [InlineData(typeof(IRepository<User>))]
    [InlineData(typeof(IListRepository<User>))]
    [InlineData(typeof(IPaginationRepository<User>))]
    public void AddRepositories_ShouldAddRepositories(Type repositoryType)
    { 
        // Arrange
        var configurationSection = Substitute.For<IConfigurationSection>();

        configurationSection[Arg.Any<string>()].ReturnsForAnyArgs("Data Source=MyDb.db");
        
        _configuration
            .GetSection("ConnectionStrings").Returns(configurationSection);
        
        _services.AddDbContext(_configuration);
        
        //Act
        _services.AddAllRepositories();

        //Assert
        var repository = _services.BuildServiceProvider().GetService(repositoryType);
        repository.Should().NotBeNull();
    }
}