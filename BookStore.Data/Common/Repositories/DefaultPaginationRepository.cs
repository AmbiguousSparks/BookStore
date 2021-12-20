using System.Linq.Expressions;
using BookStore.Application.Common.Interfaces;
using BookStore.Data.Common.Repositories.Abstracts;
using BookStore.Domain.Common.Models;
using BookStore.Domain.Common.Models.Interfaces;
using BookStore.Domain.Common.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Data.Common.Repositories;

internal class DefaultPaginationRepository<TEntity> : Repository, IPaginationRepository<TEntity>
    where TEntity : class, IEntity
{
    public DefaultPaginationRepository(IBookStoreDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<PaginationInfo<TEntity>> GetPaged(int page, int pageSize,
        CancellationToken cancellationToken = default)
    {
        var count = await DbContext.Set<TEntity>()
            .CountAsync(cancellationToken);

        var list = await DbContext.Set<TEntity>()
            .ToListAsync(cancellationToken);

        return new PaginationInfo<TEntity>(count, list);
    }

    public async Task<PaginationInfo<TEntity>> GetPaged(int page, int pageSize,
        Expression<Func<TEntity, bool>> condition, CancellationToken cancellationToken = default)
    {
        var count = await DbContext.Set<TEntity>()
            .Where(condition)
            .CountAsync(cancellationToken);

        var list = await DbContext.Set<TEntity>()
            .Where(condition)
            .ToListAsync(cancellationToken);

        return new PaginationInfo<TEntity>(count, list);
    }
}