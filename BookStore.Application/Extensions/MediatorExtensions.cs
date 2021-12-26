using BookStore.Domain.Common.Models.Interfaces;
using BookStore.Domain.Events.Interfaces;
using MediatR;

namespace BookStore.Application.Extensions;

public static class MediatorExtensions
{
    public static async Task DispatchDomainEvents(this IMediator mediator, IEntity entity,
        CancellationToken cancellationToken = default)
    {
        var tasks = PublishEvents(mediator, entity.GetEvents(), cancellationToken);
        await Task.WhenAll(tasks);
    }

    private static IEnumerable<Task> PublishEvents(IPublisher mediator, IEnumerable<IDomainEvent> domainEvents,
        CancellationToken cancellationToken)
    {
        return domainEvents.Select(domainEvent => mediator.Publish(domainEvent, cancellationToken));
    }
}