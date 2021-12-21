using BookStore.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Data.Context;

internal class BookStoreDbContext : DbContext, IBookStoreDbContext
{
    public BookStoreDbContext(DbContextOptions<BookStoreDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BookStoreDbContext).Assembly);
    }
}