using BookStore.Domain.Common.Models.Interfaces;

namespace BookStore.Domain.Common.Repositories.Interfaces;

public interface IRepository<T>
    where T : IEntity
{
    Task Create(T entity, CancellationToken cancellationToken = default);
    
    Task<T> Get(int id, CancellationToken cancellationToken = default);
}