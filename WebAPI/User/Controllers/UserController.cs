using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SampleOrg.WebAPI.User.Controllers.Requests;
using SampleOrg.WebAPI.User.Controllers.Responses;
using SampleOrg.WebAPI.User.Services.Requests;
using SampleOrg.WebAPI.Common;
using CreateUserRequest = SampleOrg.WebAPI.User.Controllers.Requests.CreateUserRequest;

namespace SampleOrg.WebAPI.User.Controllers;

using CreateUserRequest = Requests.CreateUserRequest;

public class UserController : ControllerBase
{
    [HttpGet]
    [Route("v1/users")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(FetchUserResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetUser(
        [FromServices] IMediator mediator,
        [FromQuery] FetchUserRequest request,
        CancellationToken cancellationToken
    )
    {
        return (await mediator.Send(new GetUserRequest
            {
                UserId = request.UserId
            }, cancellationToken))
            .Match(
                Ok,
                ex => ex.AsJsonResult()
            );
    }

    [HttpPost]
    [Route("v1/users")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(CreateUserResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> CreateUser(
        [FromServices] IMediator mediator,
        [FromBody] CreateUserRequest request,
        CancellationToken cancellationToken
    )
    {
        return (await mediator.Send(MapToCreateUserRequest(request), cancellationToken)).Match(
            Ok,
            ex => ex.AsJsonResult()
        );
    }

    private static Services.Requests.CreateUserRequest MapToCreateUserRequest(CreateUserRequest request)
    {
        return new Services.Requests.CreateUserRequest
        {
            Name = request.Name,
            Email = request.Email,
            ExpensesMonthly = request.ExpensesMonthly,
            SalaryMonthly = request.SalaryMonthly
        };
    }
}