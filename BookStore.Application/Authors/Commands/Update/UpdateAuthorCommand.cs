using AutoMapper;
using BookStore.Application.Authors.Common.Models;
using BookStore.Application.Authors.Events;
using BookStore.Application.Extensions;
using BookStore.Domain.Common.Exceptions;
using BookStore.Domain.Common.Repositories.Interfaces;
using BookStore.Domain.Models.Books;
using MediatR;
using OneOf;

namespace BookStore.Application.Authors.Commands.Update;

public class UpdateAuthorCommand : AuthorInDto, IRequest<OneOf<Unit, EntityNotFound, InvalidProperty>>
{
    public int Id { get; set; }
    
    internal class UpdateAuthorCommandHandler 
        : IRequestHandler<UpdateAuthorCommand, OneOf<Unit, EntityNotFound, InvalidProperty>>
    {
        private readonly IMediator _mediator;
        private readonly IRepository<Author> _repository;
        private readonly IMapper _mapper;

        public UpdateAuthorCommandHandler(IMediator mediator, IRepository<Author> repository, IMapper mapper)
        {
            _mediator = mediator;
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<OneOf<Unit, EntityNotFound, InvalidProperty>> 
            Handle(UpdateAuthorCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var author = await _repository.Get(request.Id, cancellationToken);

                if (author is null)
                    return new EntityNotFound(nameof(Author));

                author = _mapper.Map<Author>(request);

                await _repository.Update(author, cancellationToken);
                
                author.AddDomainEvent(new AuthorUpdatedEvent
                {
                    Id = author.Id
                });

                await _mediator.DispatchDomainEvents(author, cancellationToken);

                return Unit.Value;

            }
            catch (ArgumentNullException e)
            {
                return new InvalidProperty(nameof(Author), e.Message);
            }
        }
    }
}