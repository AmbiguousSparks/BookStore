using System.Runtime.CompilerServices;
using BookStore.Domain.Common.Models.Interfaces;
using BookStore.Domain.Events.Interfaces;

namespace BookStore.Domain.Common.Models;

public abstract class Entity<TId> : IEntity
{
    private readonly ICollection<IDomainEvent> _domainEvents = new List<IDomainEvent>();
    public TId Id { get; } = default!;

    protected Entity(TId id) : this() =>
        Id = id;

    protected Entity()
    {
    }

    public void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
    
    public void RemoveDomainEvent(IDomainEvent domainEvent) => _domainEvents.Remove(domainEvent);

    public IEnumerable<IDomainEvent> GetEvents() => _domainEvents;

    protected T ValidateNull<T>(T property, [CallerMemberName] string name = "") =>
        property ?? throw new ArgumentNullException(name);

    protected decimal ValidateZero(decimal value, [CallerMemberName] string name = "") =>
        value > decimal.Zero ? value : throw new ArgumentNullException(name);
    
    protected int ValidateZero(int value, [CallerMemberName] string name = "") =>
        value > 0 ? value : throw new ArgumentNullException(name);
    
    protected int ValidateNegative(int value, [CallerMemberName] string name = "") =>
        value > -1 ? value : throw new ArgumentNullException(name);

    protected string ValidateNullOrEmpty(string property, [CallerMemberName] string name = "") =>
        string.IsNullOrEmpty(property) ? throw new ArgumentNullException(name) : property;
}