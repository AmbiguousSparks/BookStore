using BookStore.Domain.Events.Interfaces;
using MediatR;

namespace BookStore.Application.Authors.Events;

public class AuthorUpdatedEvent : IDomainEvent, INotification
{
    public int Id { get; set; }
}