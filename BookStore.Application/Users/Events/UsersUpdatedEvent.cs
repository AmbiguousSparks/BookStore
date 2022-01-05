using BookStore.Domain.Common.Services;
using BookStore.Domain.Events.Interfaces;
using MediatR;

namespace BookStore.Application.Users.Events;

internal class UsersUpdatedEvent : IDomainEvent, INotification
{
    internal class UsersUpdatedEventHandler : IDomainEventHandler<UsersUpdatedEvent>,
        INotificationHandler<UsersUpdatedEvent>
    {
        private readonly ICacheService _cacheService;

        public UsersUpdatedEventHandler(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        public async Task Handle(UsersUpdatedEvent notification, CancellationToken cancellationToken)
        {
            var keys = _cacheService.GetKeys("Users*");
            var tasks = new List<Task>(keys.Count());
            
            tasks.AddRange(keys.Select(key => _cacheService.RemoveAsync(key, cancellationToken)));

            await Task.WhenAll(tasks);
        }
    }
}