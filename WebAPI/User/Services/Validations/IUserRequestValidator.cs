using System.Threading;
using System.Threading.Tasks;
using LanguageExt;
using SampleOrg.WebAPI.Common;
using SampleOrg.WebAPI.User.Services.Requests;

namespace SampleOrg.WebAPI.User.Services.Validations;

public interface IUserRequestValidator
{
    Either<Problem, ValidatedGetUserRequest> Validate(GetUserRequest getUserRequest);

    Task<Either<Problem, ValidatedCreateUserRequest>> Validate(CreateUserRequest createUserRequest,
        CancellationToken cancellationToken);
}