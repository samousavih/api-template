using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using LanguageExt;
using SampleOrg.WebAPI.Account.Services.Validations;
using SampleOrg.WebAPI.Common;
using SampleOrg.WebAPI.User.Repository;

namespace SampleOrg.WebAPI.Account.Repository;

public class AccountValidatorRepository : IAccountValidationRepository
{
    private readonly IDalConnection _dalConnection;

    public AccountValidatorRepository(IDalConnection dalConnection)
    {
        _dalConnection = dalConnection;
    }

    public async Task<Either<Problem, UserFinancialDetail>> FetchUserFinancialDetails(Guid userId,
        CancellationToken cancellationToken)
    {
        await using var connection = _dalConnection.Create();
        var parameters = new
        {
            UserId = userId
        };
        var commandDef = new CommandDefinition(
            $@"SELECT 
                    user_id as {nameof(UserFinancialDetail.UserId)},
                    salary_monthly as {nameof(UserFinancialDetail.SalaryMonthly)},
                    expenses_monthly as {nameof(UserFinancialDetail.ExpensesMonthly)}
                    FROM financial_details
                    WHERE user_id=@UserId and status_code = '{UserStatusCode.Active.ToString()}';",
            parameters,
            cancellationToken: cancellationToken
        );

        var userFinancialDetails = (await connection.QueryAsync<UserFinancialDetail>(commandDef)).ToList();

        return userFinancialDetails.First();
    }
}