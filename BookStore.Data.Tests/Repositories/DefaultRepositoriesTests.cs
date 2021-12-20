using System;
using System.Threading;
using System.Threading.Tasks;
using BookStore.Application.Common.Interfaces;
using BookStore.Data.Common.Repositories;
using BookStore.Domain.Models.Books;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using Xunit;

namespace BookStore.Data.Tests.Repositories;

public class DefaultRepositoriesTests
{
    private readonly IBookStoreDbContext _context = Substitute.For<IBookStoreDbContext>();
    
    [Fact]
    public async Task Create_ShouldCreateEntityCorrectly()
    {
        //Arrange
        var repository = new DefaultRepository<Author>(_context);

        //Act
        await repository.Create(new Author("Daniel", new DateOnly(2021, 1, 21)));

        //Assert
        await _context.ReceivedWithAnyArgs(1)
            .Set<Author>()
            .AddAsync(Arg.Any<Author>(), Arg.Any<CancellationToken>());
        await _context.ReceivedWithAnyArgs(1)
            .SaveChangesAsync(Arg.Any<CancellationToken>());

    }
}