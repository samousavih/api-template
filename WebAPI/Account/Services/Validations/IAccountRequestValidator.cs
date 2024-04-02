using System.Threading;
using System.Threading.Tasks;
using LanguageExt;
using SampleOrg.WebAPI.Account.Services.Requests;
using SampleOrg.WebAPI.Common;

namespace SampleOrg.WebAPI.Account.Services.Validations;

public interface IAccountRequestValidator

{
    Task<Either<Problem, ValidatedCreateAccountRequest>> Validate(CreateAccountRequest createAccountRequest,
        CancellationToken cancellationToken);

    Either<Problem, ValidatedFetchAccountByUserRequest> Validate(FetchAccountsByUserRequest fetchAccountsByUserRequest);
}