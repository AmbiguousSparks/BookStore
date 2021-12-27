using AutoMapper;
using BookStore.Application.Authors.Common.Models;
using BookStore.Domain.Common.Models;
using BookStore.Domain.Common.Repositories.Interfaces;
using BookStore.Domain.Models.Books;
using MediatR;

namespace BookStore.Application.Authors.Queries.Get;

public class GetPagedAuthorsQuery : IRequest<PaginationInfo<AuthorDto>>
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public string SortColumn { get; set; } = default!;

    internal class GetPagedAuthorQueryHandler : IRequestHandler<GetPagedAuthorsQuery, PaginationInfo<AuthorDto>>
    {
        private readonly IPaginationRepository<Author> _repository;
        private readonly IMapper _mapper;

        public GetPagedAuthorQueryHandler(IPaginationRepository<Author> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PaginationInfo<AuthorDto>> Handle(GetPagedAuthorsQuery request,
            CancellationToken cancellationToken)
        {
            var paginationInfo = await
                _repository.GetPaged(request.Page, request.PageSize, request.SortColumn, cancellationToken);

            return new PaginationInfo<AuthorDto>(paginationInfo.Total,
                _mapper.Map<IEnumerable<AuthorDto>>(paginationInfo.PaginatedList));
        }
    }
}