using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using LanguageExt;
using SampleOrg.WebAPI.Account.Repository.Dto;
using SampleOrg.WebAPI.Account.Services;
using SampleOrg.WebAPI.Account.Services.Handlers;
using SampleOrg.WebAPI.Account.Services.Validations;
using SampleOrg.WebAPI.Common;

namespace SampleOrg.WebAPI.Account.Repository;

public class AccountRepository : IAccountRepository
{
    private readonly IDalConnection _dalConnection;

    public AccountRepository(IDalConnection dalConnection)
    {
        _dalConnection = dalConnection;
    }

    public async Task<Either<Problem, Guid>> CreateAccount(ValidatedCreateAccountRequest validatedCreateAccountRequest,
        CancellationToken cancellationToken)
    {
        await using var connection = _dalConnection.Create();
        var newAccountId = Guid.NewGuid();
        var parameters = new
        {
            AccountId = newAccountId,
            validatedCreateAccountRequest.UserId,
            validatedCreateAccountRequest.AccountNumber,
            validatedCreateAccountRequest.Description,
            StatusCode = AccountStatusCode.Active.ToString()
        };
        var commandDef = new CommandDefinition(
            "insert into accounts (user_id, account_id, account_number,description, status_code) VALUES (@UserId, @AccountId, @AccountNumber, @Description, @StatusCode);",
            parameters,
            cancellationToken: cancellationToken
        );

        await connection.ExecuteAsync(commandDef);
        return newAccountId;
    }

    public async Task<Either<Problem, UserAccounts>> FetchAccount(
        ValidatedFetchAccountByUserRequest validatedFetchAccountByUserRequest,
        CancellationToken cancellationToken)
    {
        await using var connection = _dalConnection.Create();
        var parameters = new
        {
            validatedFetchAccountByUserRequest.UserId
        };
        var commandDef = new CommandDefinition(
            $@"SELECT 
                    user_id as {nameof(AccountsByUserDto.UserId)},
                    account_id as {nameof(AccountsByUserDto.AccountId)},
                    account_number as {nameof(AccountsByUserDto.AccountNumber)},
                    description as {nameof(AccountsByUserDto.Description)}
                    FROM accounts 
                    WHERE user_id=@UserId and status_code = '{AccountStatusCode.Active.ToString()}';",
            parameters,
            cancellationToken: cancellationToken
        );

        var accountsByUser = (await connection.QueryAsync<AccountsByUserDto>(commandDef)).ToList();

        return new UserAccounts
        {
            UserId = validatedFetchAccountByUserRequest.UserId,
            Accounts = accountsByUser.Select(accountDto => new AccountDetail
            {
                UserId = accountDto.UserId,
                AccountNumber = accountDto.AccountNumber,
                Description = accountDto.Description
            })
        };
    }
}