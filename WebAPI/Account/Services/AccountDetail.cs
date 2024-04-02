using System;
using System.Collections.Generic;

namespace SampleOrg.WebAPI.Account.Services;

public class AccountDetail
{
    public Guid UserId { get; set; }
    public string AccountNumber { get; set; }
    public string Description { get; set; }
}

public class UserAccounts
{
    public Guid UserId { get; set; }

    public IEnumerable<AccountDetail> Accounts { get; set; }
}