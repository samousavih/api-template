using System;
using System.Threading;
using System.Threading.Tasks;
using LanguageExt;
using SampleOrg.WebAPI.Account.Services.Validations;
using SampleOrg.WebAPI.Common;

namespace SampleOrg.WebAPI.Account.Services.Handlers;

public interface IAccountRepository
{
    Task<Either<Problem, Guid>> CreateAccount(ValidatedCreateAccountRequest validatedCreateAccountRequest,
        CancellationToken cancellationToken);

    Task<Either<Problem, UserAccounts>> FetchAccount(
        ValidatedFetchAccountByUserRequest validatedFetchAccountByUserRequest, CancellationToken cancellationToken);
}