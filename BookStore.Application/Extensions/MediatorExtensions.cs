using BookStore.Domain.Common.Models.Interfaces;
using BookStore.Domain.Events.Interfaces;
using MediatR;

namespace BookStore.Application.Extensions;

public static class MediatorExtensions
{
    public static async Task DispatchDomainEvents(this IMediator mediator, IEntity entity)
    {
        var tasks = PublishEvents(mediator, entity.GetEvents());
        await Task.WhenAll(tasks);
    }

    private static IEnumerable<Task> PublishEvents(IPublisher mediator, IEnumerable<IDomainEvent> domainEvents)
    {
        foreach (var domainEvent in domainEvents)
            yield return mediator.Publish(domainEvent);
    }
}