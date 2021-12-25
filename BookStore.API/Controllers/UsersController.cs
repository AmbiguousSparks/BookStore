using BookStore.API.Common.Auth;
using BookStore.Application.Users.Commands.Authenticate;
using BookStore.Application.Users.Commands.Create;
using BookStore.Application.Users.Common.Models;
using BookStore.Application.Users.Queries.GetUsers;
using BookStore.Domain.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.API.Controllers;

[ApiController, Route("api/[controller]"), Authorize(Roles = AuthConstants.AdministratorRole)]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost, AllowAnonymous]
    public async Task<IActionResult> Create([FromBody] CreateCommand createCommand,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(createCommand, cancellationToken);
        return response.Match<IActionResult>(
            _ => CreatedAtAction(nameof(GetAll), response.Value),
            BadRequest,
            BadRequest);
    }

    [HttpPost("login"), AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] AuthenticateCommand command, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(command, cancellationToken);
        return response.Match<IActionResult>(Ok, BadRequest);
    }

    [HttpGet]
    public async Task<IEnumerable<UserOutDto>> GetAll([FromQuery] GetAllUsersQuery query, CancellationToken cancellationToken)
    {
        return await _mediator.Send(query, cancellationToken);
    }
    
    [HttpGet("paged")]
    public async Task<PaginationInfo<UserOutDto>> GetPaged([FromQuery] GetPagedUsersQuery query, CancellationToken cancellationToken)
    {
        return await _mediator.Send(query, cancellationToken);
    }
}