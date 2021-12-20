using System.Linq.Expressions;
using BookStore.Application.Common.Interfaces;
using BookStore.Data.Common.Repositories.Abstracts;
using BookStore.Domain.Common.Models.Interfaces;
using BookStore.Domain.Common.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Data.Common.Repositories;

internal class DefaultRepository<TEntity> : Repository, IRepository<TEntity>
    where TEntity : class, IEntity
{
    public DefaultRepository(IBookStoreDbContext dbContext)
        : base(dbContext)
    {
    }

    public async Task Create(TEntity entity, CancellationToken cancellationToken = default)
    {
        await DbContext.Set<TEntity>()
            .AddAsync(entity, cancellationToken);

        await DbContext.SaveChangesAsync(cancellationToken);
    }

    public ValueTask<TEntity> Get(int id, CancellationToken cancellationToken = default)
    {
        return DbContext.Set<TEntity>().FindAsync(new object?[] { id }, cancellationToken: cancellationToken)!;
    }

    public Task<bool> Exists(Expression<Func<TEntity, bool>> condition, CancellationToken cancellationToken = default)
    {
        return DbContext.Set<TEntity>().AnyAsync(condition, cancellationToken);
    }
}