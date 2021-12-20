using BookStore.Application.Common.Interfaces;

namespace BookStore.Data.Common.Repositories.Abstracts;

internal abstract class Repository
{
    protected readonly IBookStoreDbContext DbContext;

    protected Repository(IBookStoreDbContext dbContext)
    {
        DbContext = dbContext;
    }
}