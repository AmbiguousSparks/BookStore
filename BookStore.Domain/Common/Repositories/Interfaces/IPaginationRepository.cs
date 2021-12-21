using System.Linq.Expressions;
using BookStore.Domain.Common.Models;
using BookStore.Domain.Common.Models.Interfaces;

namespace BookStore.Domain.Common.Repositories.Interfaces;

public interface IPaginationRepository<T>
    where T : IEntity
{
    Task<PaginationInfo<T>> GetPaged(int page, int pageSize, string sortColumn,
        CancellationToken cancellationToken = default);

    Task<PaginationInfo<T>> GetPaged(int page, int pageSize, Expression<Func<T, bool>> condition,
        string sortColumn, CancellationToken cancellationToken = default);
}