using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using LanguageExt;
using SampleOrg.WebAPI.Common;
using SampleOrg.WebAPI.User.Repository.Dto;
using SampleOrg.WebAPI.User.Services.Handlers;
using SampleOrg.WebAPI.User.Services.Validations;
using FinancialDetails = SampleOrg.WebAPI.User.Services.Responses.FinancialDetails;
using Responses_FinancialDetails = SampleOrg.WebAPI.User.Services.Responses.FinancialDetails;
using Responses_UserDetails = SampleOrg.WebAPI.User.Services.Responses.UserDetails;
using UserDetails = SampleOrg.WebAPI.User.Services.Responses.UserDetails;

namespace SampleOrg.WebAPI.User.Repository;

public class UserRepository : IUserRepository
{
    private readonly IDalConnection _dalConnection;

    public UserRepository(IDalConnection dalConnection)
    {
        _dalConnection = dalConnection;
    }


    public async Task<Either<Problem, Responses_UserDetails>> FetchUser(ValidatedGetUserRequest validatedGetUserRequest,
        CancellationToken cancellationToken)
    {
        await using var connection = _dalConnection.Create();
        var parameters = new
        {
            validatedGetUserRequest.UserId
        };
        var commandDef = new CommandDefinition(
            $@"SELECT 
                    u.user_id as {nameof(UserDetailsDto.UserId)},
                    u.name as {nameof(UserDetailsDto.Name)},
                    u.email as {nameof(UserDetailsDto.Email)},
                    fd.salary_monthly as {nameof(UserDetailsDto.SalaryMonthly)},
                    fd.expenses_monthly as {nameof(UserDetailsDto.ExpensesMonthly)}
                    FROM users u INNER JOIN financial_details fd ON u.user_id = fd.user_id
                    WHERE u.user_id=@UserId and u.status_code = '{UserStatusCode.Active.ToString()}'; ",
            parameters,
            cancellationToken: cancellationToken
        );

        var users = (await connection.QueryAsync<UserDetailsDto>(commandDef)).ToList();


        if (!users.Any()) return Problems.UserNotExist(validatedGetUserRequest.UserId);

        var user = users.First();

        return new Responses_UserDetails
        {
            UserId = user.UserId,
            Name = user.Name,
            Email = user.Email,
            FinancialDetails = new Responses_FinancialDetails
            {
                SalaryMonthly = user.SalaryMonthly,
                ExpensesMonthly = user.ExpensesMonthly
            }
        };
    }

    public async Task<Either<Problem, Guid>> CreateUser(ValidatedCreateUserRequest validatedCreateUserRequest,
        CancellationToken cancellationToken)
    {
        await using var connection = _dalConnection.Create();
        var newUserId = Guid.NewGuid();
        var parameters = new
        {
            UserId = newUserId,
            validatedCreateUserRequest.Name,
            validatedCreateUserRequest.Email,
            validatedCreateUserRequest.SalaryMonthly,
            validatedCreateUserRequest.ExpensesMonthly,
            StatusCode = UserStatusCode.Active.ToString()
        };
        var commandDef = new CommandDefinition(
            @"insert into users (user_id, name, email, status_code) VALUES (@UserId, @Name, @Email, @StatusCode);
                          insert into financial_details (user_id, salary_monthly, expenses_monthly, status_code) VALUES (@UserId, @SalaryMonthly, @ExpensesMonthly, @StatusCode);",
            parameters,
            cancellationToken: cancellationToken
        );

        await connection.ExecuteAsync(commandDef);
        return newUserId;
    }
}