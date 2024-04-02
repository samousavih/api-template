using System;
using LanguageExt;
using MediatR;
using SampleOrg.WebAPI.Account.Controllers.Responses;
using SampleOrg.WebAPI.Common;

namespace SampleOrg.WebAPI.Account.Services.Requests;

public class FetchAccountsByUserRequest : IRequest<Either<Problem, FetchAccountsByUserResponse>>
{
    public Guid UserId { get; set; }
}