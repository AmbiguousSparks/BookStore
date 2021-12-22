using AutoMapper;
using BookStore.Application.Cache;
using BookStore.Application.Users.Common.Models;
using BookStore.Domain.Common.Repositories.Interfaces;
using BookStore.Domain.Models.Users;
using MediatR;

namespace BookStore.Application.Users.Query;

[Cached(120, nameof(GetAllUsersQuery))]
public class GetAllUsersQuery : IRequest<IEnumerable<UserOutDto>>
{
    internal class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, IEnumerable<UserOutDto>>
    {
        private readonly IMapper _mapper;
        private readonly IListRepository<User> _listRepository;
        
        public GetAllUsersQueryHandler(IMapper mapper, IListRepository<User> listRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _listRepository = listRepository ?? throw new ArgumentNullException(nameof(listRepository));
        }
        
        public async Task<IEnumerable<UserOutDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _listRepository.GetAll(cancellationToken);

            return _mapper.Map<IEnumerable<UserOutDto>>(users);
        }
    }
}