using BookStore.API.Common.Auth;
using BookStore.API.Controllers.Abstracts;
using BookStore.Application.Users.Commands.Authenticate;
using BookStore.Application.Users.Commands.Create;
using BookStore.Application.Users.Common.Models;
using BookStore.Application.Users.Queries.GetUsers;
using BookStore.Domain.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.API.Controllers;

[ApiController, Route("api/[controller]"), Authorize(AuthConstants.AdministratorPolicy)]
public class UsersController : BaseApiController
{
    public UsersController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost, AllowAnonymous]
    public async Task<IActionResult> Create([FromBody] CreateUserCommand createUserCommand,
        CancellationToken cancellationToken)
    {
        var response = await Mediator.Send(createUserCommand, cancellationToken);
        return response.Match<IActionResult>(
            _ => CreatedAtAction(nameof(GetAll), response.Value),
            BadRequest,
            BadRequest);
    }

    [HttpPost("login"), AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] AuthenticateUserCommand userCommand, CancellationToken cancellationToken)
    {
        var response = await Mediator.Send(userCommand, cancellationToken);
        return response.Match<IActionResult>(Ok, BadRequest);
    }

    [HttpGet]
    public async Task<IEnumerable<UserOutDto>> GetAll([FromQuery] GetAllUsersQuery query, CancellationToken cancellationToken)
    {
        return await Mediator.Send(query, cancellationToken);
    }
    
    [HttpGet("paged")]
    public async Task<PaginationInfo<UserOutDto>> GetPaged([FromQuery] GetPagedUsersQuery query, CancellationToken cancellationToken)
    {
        return await Mediator.Send(query, cancellationToken);
    }
}