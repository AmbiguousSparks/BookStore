using BookStore.Application.Users.Commands.CreateUser;
using BookStore.Application.Users.Common.Models;
using BookStore.Application.Users.Query;
using BookStore.Domain.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.API.Controllers;

[ApiController, Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost, AllowAnonymous]
    public async Task<IActionResult> Create([FromBody] CreateUserCommand createUserCommand,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(createUserCommand, cancellationToken);
        return response.Match<IActionResult>(
            _ => CreatedAtAction(nameof(GetAll), createUserCommand),
            BadRequest,
            BadRequest);
    }

    [HttpGet, AllowAnonymous]
    public async Task<IEnumerable<UserOutDto>> GetAll(CancellationToken cancellationToken)
    {
        return await _mediator.Send(new GetAllUsersQuery(), cancellationToken);
    }
    
    [HttpGet("paged"), AllowAnonymous]
    public async Task<PaginationInfo<UserOutDto>> GetPaged([FromQuery] GetPagedUsersQuery query, CancellationToken cancellationToken)
    {
        return await _mediator.Send(query, cancellationToken);
    }
}