using System;
using System.Threading.Tasks;
using BookStore.Application.Common.Interfaces;
using BookStore.Data.Common.Repositories;
using BookStore.Data.Context;
using BookStore.Domain.Models.Books;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace BookStore.Data.Tests.Repositories;

public class DefaultRepositoryTests
{
    private readonly IBookStoreDbContext _context;

    public DefaultRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<BookStoreDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new BookStoreDbContext(options);
    }

    [Fact]
    public async Task Create_ShouldCreateEntityCorrectly()
    {
        //Arrange
        var repository = new DefaultRepository<Author>(_context);

        var authorToCreate = new Author("Daniel", "test.png", new DateOnly(2021, 1, 21));

        //Act
        await repository.Create(authorToCreate);

        //Assert
        var author = await _context.Set<Author>().FindAsync(authorToCreate.Id);
        author.Should().NotBeNull();
        author.Should().BeEquivalentTo(authorToCreate);
    }

    [Fact]
    public async Task Get_ShouldReturnCreatedEntity()
    {
        //Arrange
        var repository = new DefaultRepository<Author>(_context);

        var authorToCreate = new Author("Daniel", "test.png", new DateOnly(2021, 1, 21));
        await repository.Create(authorToCreate);

        //Act
        var found = await repository.Get(authorToCreate.Id);

        //Assert
        found.Should().NotBeNull();
        found.Should().BeEquivalentTo(authorToCreate);
    }

    [Fact]
    public async Task Get_ShouldReturnCreatedEntityBasedOnCondition()
    {
        //Arrange
        var repository = new DefaultRepository<Author>(_context);

        var authorToCreate = new Author("Daniel", "test.png", new DateOnly(2021, 1, 21));
        await repository.Create(authorToCreate);

        //Act
        var found = await repository.Get(a => a.Name == "Daniel");

        //Assert
        found.Should().NotBeNull();
        found.Should().BeEquivalentTo(authorToCreate);
    }

    [Fact]
    public async Task Exists_ShouldReturnTrueIfTheEntityIsCreated()
    {
        //Arrange
        var repository = new DefaultRepository<Author>(_context);

        var authorToCreate = new Author("Daniel", "test.png", new DateOnly(2021, 1, 21));
        await repository.Create(authorToCreate);

        //Act
        var found = await repository.Exists(a => a.Name == "Daniel");

        //Assert
        found.Should().BeTrue();
    }

    [Fact]
    public async Task Exists_ShouldReturnFalseIfTheEntityIsCreated()
    {
        //Arrange
        var repository = new DefaultRepository<Author>(_context);

        var authorToCreate = new Author("Daniel", "test.png", new DateOnly(2021, 1, 21));
        await repository.Create(authorToCreate);

        //Act
        var found = await repository.Exists(a => a.Name == "Daniel 2");

        //Assert
        found.Should().BeFalse();
    }
}