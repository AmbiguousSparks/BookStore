using BookStore.API.Common.Auth;
using BookStore.API.Controllers.Abstracts;
using BookStore.Application.Authors.Commands.Create;
using BookStore.Application.Authors.Commands.Update;
using BookStore.Application.Authors.Queries.Get;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.API.Controllers;

[ApiController, Route("api/[controller]"), Authorize(AuthConstants.EmployeePolicy)]
public class AuthorsManagementController : BaseApiController
{
    public AuthorsManagementController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAuthorCommand authorCommand,
        CancellationToken cancellationToken)
    {
        var response = await Mediator.Send(authorCommand, cancellationToken);
        return response.Match<IActionResult>(_ => Ok(), BadRequest);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateAuthorCommand command,
        CancellationToken cancellationToken)
    {
        var response = await Mediator.Send(command, cancellationToken);
        return response.Match<IActionResult>(_ => Ok(), NotFound, BadRequest);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetAllAuthorsQuery query, CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(query, cancellationToken));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetAll(int id, CancellationToken cancellationToken)
    {
        var response = await Mediator.Send(new GetAuthorQuery {Id = id}, cancellationToken);
        return response.Match<IActionResult>(Ok, NotFound);
    }

    [HttpGet("paged")]
    public async Task<IActionResult> GetPagedAuthors([FromQuery] GetPagedAuthorsQuery query,
        CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(query, cancellationToken));
    }
}