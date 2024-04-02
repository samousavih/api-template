using System.Threading;
using System.Threading.Tasks;
using LanguageExt;
using SampleOrg.WebAPI.Common;
using SampleOrg.WebAPI.User.Services.Requests;

namespace SampleOrg.WebAPI.User.Services.Validations;

public class UserRequestValidator : IUserRequestValidator
{
    private readonly IUserValidationRepository _userValidationRepository;

    public UserRequestValidator(IUserValidationRepository userValidationRepository)
    {
        _userValidationRepository = userValidationRepository;
    }

    public Either<Problem, ValidatedGetUserRequest> Validate(GetUserRequest getUserRequest)
    {
        //This validator is just a pass-through, but more validations can be added here
        return new ValidatedGetUserRequest
        {
            UserId = getUserRequest.UserId
        };
    }

    public Task<Either<Problem, ValidatedCreateUserRequest>> Validate(CreateUserRequest createUserRequest,
        CancellationToken cancellationToken)
    {
        return _userValidationRepository.ExistUserWithEmail(createUserRequest.Email, cancellationToken)
            .BindAsync(x => CheckIsExist(x, createUserRequest))
            .MapAsync(userRequest => new ValidatedCreateUserRequest
            {
                Name = userRequest.Name,
                Email = userRequest.Email,
                ExpensesMonthly = userRequest.ExpensesMonthly,
                SalaryMonthly = userRequest.SalaryMonthly
            });
    }

    private Either<Problem, CreateUserRequest> CheckIsExist(bool exist, CreateUserRequest createUserRequest)
    {
        return exist ? Problems.UserWithSameEmailExists(createUserRequest.Email) : createUserRequest;
    }
}