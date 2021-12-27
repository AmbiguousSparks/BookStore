using BookStore.API.Common.Auth;
using BookStore.API.Controllers.Abstracts;
using BookStore.Application.Authors.Commands.Create;
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
    public async Task<IActionResult> Create([FromBody] CreateAuthorCommand authorCommand, CancellationToken cancellationToken)
    {
        var response = await Mediator.Send(authorCommand, cancellationToken);
        return response.Match<IActionResult>(_ => Ok(), BadRequest);
    }
}