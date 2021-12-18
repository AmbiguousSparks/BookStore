using System.Runtime.CompilerServices;
using BookStore.Domain.Common.Exceptions;
using BookStore.Domain.Common.Models.Interfaces;
using BookStore.Domain.Events.Interfaces;

namespace BookStore.Domain.Common.Models;

public abstract class Entity<TClass, TId> : IEntity
{
    private readonly ICollection<IDomainEvent> _domainEvents = new List<IDomainEvent>();
    public TId Id { get; } = default!;

    private readonly string _className;

    protected Entity(TId id) : this() =>
        Id = id;

    protected Entity()
    {
        _className = typeof(TClass).Name;
    }

    public void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
    
    public void RemoveDomainEvent(IDomainEvent domainEvent) => _domainEvents.Remove(domainEvent);

    public IEnumerable<IDomainEvent> GetEvents() => _domainEvents;

    protected T ValidateNull<T>(T property, [CallerMemberName] string name = "") =>
        property ?? throw PropertyNullException.Throw(_className, name);

    protected string ValidateNullOrEmpty(string property, [CallerMemberName] string name = "") =>
        string.IsNullOrEmpty(property) ? throw PropertyNullException.Throw(_className, name) : property;
}