using SampleOrg.WebAPI.Account.Controllers.Responses;

namespace SampleOrg.WebAPI.Account.Services.Mappers;

public interface IAccountResponseMapper
{
    FetchAccountsByUserResponse Map(UserAccounts userAccounts);
}