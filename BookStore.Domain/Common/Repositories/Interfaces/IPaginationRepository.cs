using System.Linq.Expressions;
using BookStore.Domain.Common.Models;
using BookStore.Domain.Common.Models.Interfaces;

namespace BookStore.Domain.Common.Repositories.Interfaces;

public interface IPaginationRepository<T>
    where T : IEntity
{
    Task<PaginationInfo<T>> GetPaged<TKey>(int page, int pageSize, Expression<Func<T, TKey>> orderBy,
        CancellationToken cancellationToken = default);

    Task<PaginationInfo<T>> GetPaged<TKey>(int page, int pageSize, Expression<Func<T, bool>> condition,
        Expression<Func<T, TKey>> orderBy, CancellationToken cancellationToken = default);
}