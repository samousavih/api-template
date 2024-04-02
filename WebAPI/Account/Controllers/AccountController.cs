using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SampleOrg.WebAPI.Account.Controllers.Requests;
using SampleOrg.WebAPI.Account.Controllers.Responses;
using SampleOrg.WebAPI.Common;

namespace SampleOrg.WebAPI.Account.Controllers;

public class AccountController : ControllerBase
{
    [HttpGet]
    [Route("v1/accounts")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(FetchAccountsByUserResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetAccountsByUser(
        [FromServices] IMediator mediator,
        [FromQuery] FetchAccountsByUserRequest request,
        CancellationToken cancellationToken
    )
    {
        return (await mediator.Send(new Services.Requests.FetchAccountsByUserRequest
        {
            UserId = request.UserId
        }, cancellationToken)).Match(
            Ok,
            ex => ex.AsJsonResult()
        );
    }

    [HttpPost]
    [Route("v1/accounts")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(CreateAccountResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> CreateAccount(
        [FromServices] IMediator mediator,
        [FromBody] CreateAccountRequest request,
        CancellationToken cancellationToken
    )
    {
        return (await mediator.Send(MapToAccountRequest(request), cancellationToken)).Match(
            Ok,
            ex => ex.AsJsonResult()
        );
    }

    private static Services.Requests.CreateAccountRequest MapToAccountRequest(CreateAccountRequest request)
    {
        return new Services.Requests.CreateAccountRequest
        {
            UserId = request.UserId,
            AccountNumber = request.AccountNumber,
            Description = request.Description
        };
    }
}