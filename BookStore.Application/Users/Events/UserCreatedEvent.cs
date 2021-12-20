using BookStore.Domain.Events.Interfaces;
using MediatR;

namespace BookStore.Application.Users.Events;

internal class UserCreatedEvent : IDomainEvent, INotification
{
}