using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.API.Controllers.Abstracts;

public class BaseApiController : ControllerBase
{
    protected IMediator Mediator { get; }

    public BaseApiController(IMediator mediator)
    {
        Mediator = mediator;
    }
}