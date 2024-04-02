using System;
using System.Collections.Generic;
using SampleOrg.WebAPI.Account.Services;

namespace SampleOrg.WebAPI.Account.Controllers.Responses;

public class FetchAccountsByUserResponse
{
    public List<AccountDetail> AccountDetails { get; set; }
    public Guid UserId { get; set; }
}