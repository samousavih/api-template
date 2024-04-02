using System.Linq;
using SampleOrg.WebAPI.Account.Controllers.Responses;

namespace SampleOrg.WebAPI.Account.Services.Mappers;

internal class AccountResponseMapper : IAccountResponseMapper
{
    public FetchAccountsByUserResponse Map(UserAccounts userAccounts)
    {
        return new FetchAccountsByUserResponse
        {
            UserId = userAccounts.UserId,
            AccountDetails = userAccounts.Accounts.ToList()
        };
    }
}