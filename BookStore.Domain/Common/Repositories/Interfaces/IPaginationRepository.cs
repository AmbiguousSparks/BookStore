using System.Linq.Expressions;
using BookStore.Domain.Common.Models;
using BookStore.Domain.Common.Models.Interfaces;

namespace BookStore.Domain.Common.Repositories.Interfaces;

public interface IPaginationRepository<T> : IRepository<T>
    where T : IEntity
{
    Task<PaginationInfo<T>> GetPaged(CancellationToken cancellationToken = default);

    Task<PaginationInfo<T>> GetPaged(Expression<Func<T, bool>> condition,
        CancellationToken cancellationToken = default);
}