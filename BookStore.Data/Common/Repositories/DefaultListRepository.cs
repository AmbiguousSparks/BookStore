using System.Linq.Expressions;
using BookStore.Application.Common.Interfaces;
using BookStore.Data.Common.Repositories.Abstracts;
using BookStore.Domain.Common.Models.Interfaces;
using BookStore.Domain.Common.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Data.Common.Repositories;

internal class DefaultListRepository<TEntity> : Repository<TEntity>, IListRepository<TEntity>
    where TEntity : class, IEntity
{
    public DefaultListRepository(IBookStoreDbContext dbContext)
        : base(dbContext)
    {
    }

    public async Task<IEnumerable<TEntity>> GetAll(CancellationToken cancellationToken = default)
    {
        return await All()
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<TEntity>> GetAll(Expression<Func<TEntity, bool>> condition,
        CancellationToken cancellationToken = default)
    {
        return await All()
            .Where(condition)
            .ToListAsync(cancellationToken);
    }
}