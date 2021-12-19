using System;
using BookStore.Application.Extensions;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BooksStore.Application.Tests.Extensions;

public class ApplicationExtensionsTests
{
    [Theory]
    [InlineData(typeof(IPipelineBehavior<IRequest, Unit>))]
    [InlineData(typeof(IMediator))]
    public void AddApplication_ShouldRegisterServices(Type serviceType)
    {
        //Arrange
        var serviceCollection = new ServiceCollection();

        //Act
        serviceCollection.AddApplication();

        //Assert
        var service = serviceCollection.BuildServiceProvider().GetService(serviceType);
        service.Should().NotBeNull();
    }
}