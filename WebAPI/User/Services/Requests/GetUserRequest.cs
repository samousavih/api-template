using System;
using LanguageExt;
using MediatR;
using SampleOrg.WebAPI.Common;
using SampleOrg.WebAPI.User.Services.Responses;

namespace SampleOrg.WebAPI.User.Services.Requests;

public class GetUserRequest : IRequest<Either<Problem, GetUserResponse>>
{
    public Guid UserId { get; set; }
}