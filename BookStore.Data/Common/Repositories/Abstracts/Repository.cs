using BookStore.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Data.Common.Repositories.Abstracts;

internal abstract class Repository<T>
    where T : class
{
    protected readonly IBookStoreDbContext DbContext;
    protected DbSet<T> All() => DbContext.Set<T>();

    protected Repository(IBookStoreDbContext dbContext)
    {
        DbContext = dbContext;
    }
}