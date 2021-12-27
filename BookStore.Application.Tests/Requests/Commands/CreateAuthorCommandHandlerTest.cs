using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BookStore.Application.Authors.Commands.Create;
using BookStore.Application.Extensions;
using BookStore.Domain.Common.Repositories.Interfaces;
using BookStore.Domain.Models.Books;
using MediatR;
using NSubstitute;
using Xunit;

namespace BookStore.Application.Tests.Requests.Commands;

public class CreateAuthorCommandHandlerTest
{
    private readonly IMediator _mediator = Substitute.For<IMediator>();
    private readonly IRepository<Author> _repository = Substitute.For<IRepository<Author>>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();

    [Fact]
    public async Task Handle_ShouldCreateNewAuthor()
    {
        //Arrange
        var author = new Author("Test", "test.png", new DateTime(2000, 8, 23));
        _mapper.Map<Author>(Arg.Any<CreateAuthorCommand>())
            .ReturnsForAnyArgs(author);
        var handler = new CreateAuthorCommand.CreateAuthorCommandHandler(_mapper, _repository, _mediator);

        //Act
        await handler.Handle(new CreateAuthorCommand
        {
            Name = "Test",
            Photo = "test.png",
            BirthDate = new DateTime(2000, 8, 23)
        }, CancellationToken.None);

        //Assert
        _mapper.ReceivedWithAnyArgs(1).Map<Author>(Arg.Any<CreateAuthorCommand>());
        await _repository.ReceivedWithAnyArgs(1).Create(Arg.Any<Author>(), Arg.Any<CancellationToken>());
        await _mediator.ReceivedWithAnyArgs(1).DispatchDomainEvents(author, Arg.Any<CancellationToken>());
    } 
}