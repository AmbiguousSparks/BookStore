using BookStore.Domain.Events.Interfaces;

namespace BookStore.Domain.Common.Models.Interfaces;

public interface IEntity
{
    void AddDomainEvent(IDomainEvent domainEvent);
    void RemoveDomainEvent(IDomainEvent domainEvent);

    IEnumerable<IDomainEvent> GetEvents();
}