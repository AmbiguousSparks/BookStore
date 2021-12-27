using BookStore.API.Common.Auth;
using BookStore.API.Controllers.Abstracts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.API.Controllers;

[ApiController, Route("api/[controller]"), Authorize(AuthConstants.EmployeePolicy)]
public class BooksManagementController : BaseApiController
{
    public BooksManagementController(IMediator mediator) : base(mediator)
    {
    }
}