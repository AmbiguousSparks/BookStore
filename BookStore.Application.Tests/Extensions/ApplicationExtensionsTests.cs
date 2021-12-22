using System;
using BookStore.Application.Extensions;
using BookStore.Domain.Common.Services;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Xunit;

namespace BookStore.Application.Tests.Extensions;

public class ApplicationExtensionsTests
{
    [Theory]
    [InlineData(typeof(IPipelineBehavior<IRequest, Unit>))]
    [InlineData(typeof(IMediator))]
    [InlineData(typeof(ICacheService))]
    [InlineData(typeof(IDistributedCache))]
    public void AddApplication_ShouldRegisterServices(Type serviceType)
    {
        //Arrange
        var serviceCollection = new ServiceCollection();
        var configuration = Substitute.For<IConfiguration>();

        //Act
        serviceCollection.AddApplication(configuration);

        //Assert
        var service = serviceCollection.BuildServiceProvider().GetService(serviceType);
        service.Should().NotBeNull();
    }
}