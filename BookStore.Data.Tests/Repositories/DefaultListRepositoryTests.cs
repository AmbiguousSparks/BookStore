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

public class DefaultListRepositoryTests
{
    private readonly IBookStoreDbContext _context;

    public DefaultListRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<BookStoreDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new BookStoreDbContext(options);
    }

    [Fact]
    public async Task GetAll_ShouldReturnAllTheEntities()
    {
        //Arrange

        var authors = new List<Author>()
        {
            new("Daniel", "test.png", new DateTime(2000, 8, 23)),
            new("Daniel 1", "test.png", new DateTime(2000, 8, 23)),
            new("Daniel 2", "test.png", new DateTime(2000, 8, 23)),
        };

        _context.Set<Author>()
            .AddRange(authors);
        await _context.SaveChangesAsync();
        var repository = new DefaultListRepository<Author>(_context);

        //Act
        var returnedAuthors = await repository.GetAll();

        //Assert
        var enumerable = returnedAuthors as Author[] ?? returnedAuthors.ToArray();
        enumerable.Should().NotBeNull();
        enumerable.Length.Should().Be(authors.Count);
        enumerable.Should().BeEquivalentTo(authors);
    }

    [Fact]
    public async Task GetAll_ShouldReturnAllTheEntitiesWithCondition()
    {
        //Arrange

        var authors = new List<Author>()
        {
            new("Daniel", "test.png", new DateTime(2000, 8, 23)),
            new("Daniel 1", "test.png", new DateTime(2000, 8, 23)),
            new("Daniel 2", "test.png", new DateTime(2000, 8, 23)),
        };

        _context.Set<Author>()
            .AddRange(authors);
        await _context.SaveChangesAsync();
        var repository = new DefaultListRepository<Author>(_context);

        //Act
        var returnedAuthors = await repository.GetAll(a => a.Name != "Daniel");

        //Assert
        var enumerable = returnedAuthors as Author[] ?? returnedAuthors.ToArray();
        enumerable.Should().NotBeNull();
        enumerable.Length.Should().Be(2);
    }
}