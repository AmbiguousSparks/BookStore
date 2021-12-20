using BookStore.Domain.Common.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace BookStore.Application.Common.Interfaces;

public interface IBookStoreDbContext : IDisposable
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    DbSet<T> Set<T>()
        where T : class;
    
    DatabaseFacade Database { get; }
}