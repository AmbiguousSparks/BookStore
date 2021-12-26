using BookStore.Domain.Events.Interfaces;
using MediatR;

namespace BookStore.Application.Authors.Events;

public class AuthorCreatedEvent : IDomainEvent, INotification
{
    
}