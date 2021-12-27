using AutoMapper;
using BookStore.Application.Authors.Common.Models;
using BookStore.Domain.Common.Exceptions;
using BookStore.Domain.Common.Repositories.Interfaces;
using BookStore.Domain.Models.Books;
using MediatR;
using OneOf;

namespace BookStore.Application.Authors.Queries.Get;

public class GetAuthorQuery : IRequest<OneOf<AuthorDto, EntityNotFound>>
{
    public int Id { get; set; }
    
    internal class GetAuthorQueryHandler : IRequestHandler<GetAuthorQuery, OneOf<AuthorDto, EntityNotFound>>
    {
        private readonly IRepository<Author> _repository;
        private readonly IMapper _mapper;

        public GetAuthorQueryHandler(IRepository<Author> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<OneOf<AuthorDto, EntityNotFound>> Handle(GetAuthorQuery request, CancellationToken cancellationToken)
        {
            if (!await _repository.Exists(a => a.Id == request.Id, cancellationToken))
                return new EntityNotFound(nameof(Author));

            return _mapper.Map<AuthorDto>(await _repository.Get(request.Id, cancellationToken));
        }
    }
}