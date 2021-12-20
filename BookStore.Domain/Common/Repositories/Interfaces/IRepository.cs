using System.Linq.Expressions;
using BookStore.Domain.Common.Models.Interfaces;
using BookStore.Domain.Models.Users;

namespace BookStore.Domain.Common.Repositories.Interfaces;

public interface IRepository<T>
    where T : IEntity
{
    Task Create(T entity, CancellationToken cancellationToken = default);
    
    ValueTask<T> Get(int id, CancellationToken cancellationToken = default);

    Task<bool> Exists(Expression<Func<T, bool>> condition, CancellationToken cancellationToken = default);
}