using System.Linq.Expressions;
using BookStore.Domain.Common.Models.Interfaces;

namespace BookStore.Domain.Common.Repositories.Interfaces;

public interface IListRepository<T> : IRepository<T>
    where T : IEntity
{
    Task<IEnumerable<T>> GetAll(CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> condition, CancellationToken cancellationToken = default);
}