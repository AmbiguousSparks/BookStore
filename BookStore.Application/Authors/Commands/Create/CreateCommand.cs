using AutoMapper;
using BookStore.Application.Authors.Common.Models;
using BookStore.Application.Authors.Events;
using BookStore.Application.Extensions;
using BookStore.Domain.Common.Exceptions;
using BookStore.Domain.Common.Repositories.Interfaces;
using BookStore.Domain.Models.Books;
using MediatR;
using OneOf;

namespace BookStore.Application.Authors.Commands.Create;

public class CreateCommand : AuthorInDto, IRequest<OneOf<Unit, InvalidProperty>>
{
    internal class CreateAuthorCommandHandler : 
        IRequestHandler<CreateCommand, OneOf<Unit, InvalidProperty>>
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Author> _authorRepository;
        private readonly IMediator _mediator;

        public CreateAuthorCommandHandler(IMapper mapper, IRepository<Author> authorRepository, IMediator mediator)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _authorRepository = authorRepository ?? throw new ArgumentNullException(nameof(authorRepository));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<OneOf<Unit, InvalidProperty>> Handle(CreateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var authorModel = _mapper.Map<Author>(request);
                authorModel.AddDomainEvent(new AuthorCreatedEvent());

                await _authorRepository.Create(authorModel, cancellationToken);

                await _mediator.DispatchDomainEvents(authorModel, cancellationToken);

                return Unit.Value;
            }
            catch (ArgumentNullException e)
            {
                return new InvalidProperty(nameof(Author), e.Message);
            }
        }
    }
}