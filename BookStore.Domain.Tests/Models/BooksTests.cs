using System;
using AutoFixture;
using BookStore.Domain.Events.Interfaces;
using BookStore.Domain.Models.Books;
using BookStore.Domain.Models.Enums;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace BookStore.Domain.Tests.Models;

public class BooksTests
{
    private readonly IFixture _fixture = new Fixture();

    #region Constructor

    [Fact]
    public void Ctor_ShouldCreateNewBookWhenValidParameters()
    {
        //Arrange
        var author = _fixture.Build<Author>()
            .FromFactory(() => new Author("Daniel", new DateOnly(2000, 8, 23)))
            .With(a => a.Name, "Daniel")
            .With(b => b.BirthDate, new DateOnly(2000, 8, 23))
            .Create();

        var category = _fixture.Build<Category>()
            .FromFactory(() => new Category("Action"))
            .With(c => c.CategoryName, "Action")
            .Create();

        const string title = "Test Book", description = "Just a Test Book", language = "Portuguese", isbn = "978-855500";


        //Act
        var book = new Book(title, description, isbn, author, language,
            category, new DateOnly(2021, 12, 18),
            BookCoverType.Common, 1, 29.99m);

        //Assert
        book.Title.Should().Be(title);
        book.Author.Name.Should().Be(author.Name);
        book.Category.CategoryName.Should().Be(category.CategoryName);
        book.ReleaseDate.Should().Be(new DateOnly(2021, 12, 18));
        book.Description.Should().Be(description);
        book.Language.Should().Be(language);
        book.Isbn.Should().Be(isbn);
        book.CoverType.Should().Be(BookCoverType.Common);
        book.Edition.Should().Be(1);
        book.Price.Should().Be(29.99m);
    }
    
    [Fact]
    public void Ctor_ShouldCreateNewBookWhenValidParametersAndId()
    {
        //Arrange
        var author = _fixture.Build<Author>()
            .FromFactory(() => new Author(1, "Daniel", new DateOnly(2000, 8, 23)))
            .With(a => a.Name, "Daniel")
            .With(b => b.BirthDate, new DateOnly(2000, 8, 23))
            .Create();

        var category = _fixture.Build<Category>()
            .FromFactory(() => new Category(1, "Action"))
            .With(c => c.CategoryName, "Action")
            .Create();

        const string title = "Test Book", description = "Just a Test Book", language = "Portuguese", isbn = "978-855500";


        //Act
        var book = new Book(1, title, description, isbn, author, language,
            category, new DateOnly(2021, 12, 18),
            BookCoverType.Common, 1, 29.99m);

        //Assert
        book.Title.Should().Be(title);
        book.Author.Id.Should().Be(1);
        book.Category.Id.Should().Be(1);
        book.ReleaseDate.Should().Be(new DateOnly(2021, 12, 18));
        book.Description.Should().Be(description);
        book.Language.Should().Be(language);
        book.Isbn.Should().Be(isbn);
        book.CoverType.Should().Be(BookCoverType.Common);
        book.Edition.Should().Be(1);
        book.Price.Should().Be(29.99m);
        book.Id.Should().Be(1);
    }
    
    [Fact]
    public void Ctor_ShouldThrowWhenCreatingWithInvalidParameters()
    {
        //Arrange
        var author = _fixture.Build<Author>()
            .FromFactory(() => new Author("Daniel", new DateOnly(2000, 8, 23)))
            .With(a => a.Name, "Daniel")
            .With(b => b.BirthDate, new DateOnly(2000, 8, 23))
            .Create();

        var category = _fixture.Build<Category>()
            .FromFactory(() => new Category("Action"))
            .With(c => c.CategoryName, "Action")
            .Create();

        const string title = "", description = "Just a Test Book", language = "Portuguese", isbn = "978-855500";


        //Act
        var act = () => new Book(1, title, description, isbn, author, language,
            category, new DateOnly(2021, 12, 18),
            BookCoverType.Common, 1, 29.99m);

        //Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'Title')");

    }

    #endregion

    #region Events

    [Fact]
    public void AddDomainEvent_ShouldAddNewEvent()
    {
        //Arrange
        var author = _fixture.Build<Author>()
            .FromFactory(() => new Author("Daniel", new DateOnly(2000, 8, 23)))
            .With(a => a.Name, "Daniel")
            .With(b => b.BirthDate, new DateOnly(2000, 8, 23))
            .Create();

        var category = _fixture.Build<Category>()
            .FromFactory(() => new Category("Action"))
            .With(c => c.CategoryName, "Action")
            .Create();

        const string title = "Test Book", description = "Just a Test Book", language = "Portuguese", isbn = "978-855500";
        
        var book = new Book(title, description, isbn, author, language,
            category, new DateOnly(2021, 12, 18),
            BookCoverType.Common, 1, 29.99m);

        var @event = Substitute.For<IDomainEvent>();

        //Act
        book.AddDomainEvent(@event);

        //Assert
        book.GetEvents().Should().Contain(@event);
    }
    
    [Fact]
    public void RemoveDomainEvent_ShouldRemoveEvent()
    {
        //Arrange
        var author = _fixture.Build<Author>()
            .FromFactory(() => new Author("Daniel", new DateOnly(2000, 8, 23)))
            .With(a => a.Name, "Daniel")
            .With(b => b.BirthDate, new DateOnly(2000, 8, 23))
            .Create();

        var category = _fixture.Build<Category>()
            .FromFactory(() => new Category("Action"))
            .With(c => c.CategoryName, "Action")
            .Create();

        const string title = "Test Book", description = "Just a Test Book", language = "Portuguese", isbn = "978-855500";
        
        var book = new Book(title, description, isbn, author, language,
            category, new DateOnly(2021, 12, 18),
            BookCoverType.Common, 1, 29.99m);

        var @event = Substitute.For<IDomainEvent>();
        book.AddDomainEvent(@event);

        //Act
        book.RemoveDomainEvent(@event);
        
        //Assert
        book.GetEvents().Should().BeEmpty();
    }
    
    [Fact]
    public void GetEvents_ShouldReturnEvents()
    {
        //Arrange
        var author = _fixture.Build<Author>()
            .FromFactory(() => new Author("Daniel", new DateOnly(2000, 8, 23)))
            .With(a => a.Name, "Daniel")
            .With(b => b.BirthDate, new DateOnly(2000, 8, 23))
            .Create();

        var category = _fixture.Build<Category>()
            .FromFactory(() => new Category("Action"))
            .With(c => c.CategoryName, "Action")
            .Create();

        const string title = "Test Book", description = "Just a Test Book", language = "Portuguese", isbn = "978-855500";
        
        var book = new Book(title, description, isbn, author, language,
            category, new DateOnly(2021, 12, 18),
            BookCoverType.Common, 1, 29.99m);

        var @event1 = Substitute.For<IDomainEvent>();
        var @event2 = Substitute.For<IDomainEvent>();
        
        book.AddDomainEvent(@event1);
        book.AddDomainEvent(@event2);
        
        //Act
        var events = book.GetEvents();
        
        //Assert
        events.Should().HaveCount(2);
    }
    

    #endregion
}