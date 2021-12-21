using AutoMapper;
using BookStore.Application.Users.Common.Models;
using BookStore.Domain.Common.Models;
using BookStore.Domain.Common.Repositories.Interfaces;
using BookStore.Domain.Models.Users;
using MediatR;

namespace BookStore.Application.Users.Query;

public class GetPagedUsersQuery : IRequest<PaginationInfo<UserOutDto>>
{
    public int Page { get; set; }
    public int PageSize { get; set; }

    public string SortColumn { get; set; } = default!;
    
    internal class GetPagedUsersQueryHandler : IRequestHandler<GetPagedUsersQuery, PaginationInfo<UserOutDto>>
    {
        private readonly IMapper _mapper;
        private readonly IPaginationRepository<User> _paginationRepository
            ;
        
        public GetPagedUsersQueryHandler(IMapper mapper, IPaginationRepository<User> paginationRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _paginationRepository = paginationRepository ?? throw new ArgumentNullException(nameof(paginationRepository));
        }
        
        public async Task<PaginationInfo<UserOutDto>> Handle(GetPagedUsersQuery request, CancellationToken cancellationToken)
        {
            var pagedUsers = await _paginationRepository.GetPaged(
                request.Page, 
                request.PageSize,
                request.SortColumn,
                cancellationToken);

            var usersOut = _mapper.Map<IEnumerable<UserOutDto>>(pagedUsers.PaginatedList);

            return new PaginationInfo<UserOutDto>(pagedUsers.Total, usersOut);
        }
    }
}