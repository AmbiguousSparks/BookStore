using AutoMapper;
using BookStore.Application.Authors.Common.Models;
using BookStore.Application.Cache;
using BookStore.Domain.Common.Repositories.Interfaces;
using BookStore.Domain.Models.Books;
using MediatR;

namespace BookStore.Application.Authors.Queries.Get;

[Cached(120, nameof(GetAllAuthorsQuery))]
public class GetAllAuthorsQuery : IRequest<IEnumerable<AuthorDto>>
{
    internal class GetAllAuthorsQueryHandler : IRequestHandler<GetAllAuthorsQuery, IEnumerable<AuthorDto>>
    {
        private readonly IListRepository<Author> _repository;
        private readonly IMapper _mapper;

        public GetAllAuthorsQueryHandler(IListRepository<Author> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AuthorDto>> Handle(GetAllAuthorsQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<IEnumerable<AuthorDto>>(await _repository.GetAll(cancellationToken));
        }
    }
}