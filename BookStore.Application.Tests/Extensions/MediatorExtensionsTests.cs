using System.Collections.Generic;
using System.Threading.Tasks;
using BookStore.Application.Extensions;
using BookStore.Domain.Common.Models.Interfaces;
using BookStore.Domain.Events.Interfaces;
using MediatR;
using NSubstitute;
using Xunit;

namespace BookStore.Application.Tests.Extensions;

public class MediatorExtensionsTests
{
    [Fact]
    public async Task DispatchDomainEvents_ShouldPublishDomainEvents()
    {
        //Arrange
        var mediator = Substitute.For<IMediator>();
        var entity = Substitute.For<IEntity>();

        var domainEvent = Substitute.For<IDomainEvent>();
        
        entity.GetEvents().Returns(new List<IDomainEvent>
        {
            domainEvent
        });

        //Act
        await mediator.DispatchDomainEvents(entity);

        //Assert
        entity.ReceivedWithAnyArgs(1).GetEvents();
        await mediator.ReceivedWithAnyArgs(1).Publish(Arg.Any<IDomainEvent>());
    }
}