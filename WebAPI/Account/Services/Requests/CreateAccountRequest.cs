using System;
using LanguageExt;
using MediatR;
using SampleOrg.WebAPI.Account.Services.Responses;
using SampleOrg.WebAPI.Common;

namespace SampleOrg.WebAPI.Account.Services.Requests;

public class CreateAccountRequest : IRequest<Either<Problem, CreateAccountResponse>>
{
    public Guid UserId { get; set; }
    public string AccountNumber { get; set; }
    public string Description { get; set; }
}