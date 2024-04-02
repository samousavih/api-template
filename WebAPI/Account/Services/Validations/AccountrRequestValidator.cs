using System.Threading;
using System.Threading.Tasks;
using LanguageExt;
using SampleOrg.WebAPI.Account.Services.Requests;
using SampleOrg.WebAPI.Common;

namespace SampleOrg.WebAPI.Account.Services.Validations;

public class AccountRequestValidator : IAccountRequestValidator
{
    private readonly IAccountValidationRepository _accountValidationRepository;

    public AccountRequestValidator(IAccountValidationRepository accountValidationRepository)
    {
        _accountValidationRepository = accountValidationRepository;
    }

    public Task<Either<Problem, ValidatedCreateAccountRequest>> Validate(CreateAccountRequest createAccountRequest,
        CancellationToken cancellationToken)
    {
        return _accountValidationRepository.FetchUserFinancialDetails(createAccountRequest.UserId, cancellationToken)
            .BindAsync(CheckUserSalary)
            .MapAsync(_ => new ValidatedCreateAccountRequest
            {
                UserId = createAccountRequest.UserId,
                AccountNumber = createAccountRequest.AccountNumber,
                Description = createAccountRequest.Description
            });
    }

    public Either<Problem, ValidatedFetchAccountByUserRequest> Validate(
        FetchAccountsByUserRequest fetchAccountsByUserRequest)
    {
        //This validator is just a pass-through, but more validations can be added here 
        return new ValidatedFetchAccountByUserRequest
        {
            UserId = fetchAccountsByUserRequest.UserId
        };
    }

    private Either<Problem, bool> CheckUserSalary(UserFinancialDetail userFinancialDetail)
    {
        return userFinancialDetail.SalaryMonthly - userFinancialDetail.ExpensesMonthly < 1_000
            ? Problems.NotEnoughSalary(userFinancialDetail.UserId)
            : true;
    }
}