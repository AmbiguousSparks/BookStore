using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Application.Common.Interfaces;
using BookStore.Data.Common.Repositories;
using BookStore.Data.Context;
using BookStore.Domain.Models.Books;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace BookStore.Data.Tests.Repositories;

public class DefaultPaginationRepositoryTests
{
    private readonly IBookStoreDbContext _context;

    public DefaultPaginationRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<BookStoreDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new BookStoreDbContext(options);
    }

    [Fact]
    public async Task GetPAged_ShouldReturnAllTheEntitiesPaged()
    {
        //Arrange

        var authors = new List<Author>
        {
            new("Daniel 2", new DateOnly(2000, 8, 23)),
            new("Daniel", new DateOnly(2000, 8, 23)),
            new("Daniel 1", new DateOnly(2000, 8, 23)),
        };

        _context.Set<Author>()
            .AddRange(authors);
        await _context.SaveChangesAsync();
        var repository = new DefaultPaginationRepository<Author>(_context);

        //Act
        var pagedAuthors = await repository.GetPaged(1, 2, a => a.Name);

        //Assert
        var enumerable = pagedAuthors.PaginatedList as Author[] ?? pagedAuthors.PaginatedList.ToArray();
        enumerable.Should().NotBeNull();
        enumerable.Length.Should().Be(2);
        enumerable.First().Name.Should().Be("Daniel");
        pagedAuthors.Total.Should().Be(3);
    }

    [Fact]
    public async Task GetPaged_ShouldReturnAllTheEntitiesPagedBasedOnCondition()
    {
        //Arrange

        var authors = new List<Author>
        {
            new("Daniel", new DateOnly(2000, 8, 23)),
            new("Daniel 1", new DateOnly(2000, 8, 23)),
            new("Daniel 2", new DateOnly(2000, 8, 23)),
        };

        _context.Set<Author>()
            .AddRange(authors);
        await _context.SaveChangesAsync();
        var repository = new DefaultPaginationRepository<Author>(_context);

        //Act
        var pagedAuthors = await repository.GetPaged(1, 1, a => a.Name != "Daniel",
            author => author.Name);

        //Assert
        var enumerable = pagedAuthors.PaginatedList as Author[] ?? pagedAuthors.PaginatedList.ToArray();
        enumerable.Should().NotBeNull();
        enumerable.Length.Should().Be(1);
        pagedAuthors.Total.Should().Be(2);
    }
}