using System.Threading;
using System.Threading.Tasks;
using LanguageExt;
using SampleOrg.WebAPI.Common;

namespace SampleOrg.WebAPI.User.Services.Validations;

public interface IUserValidationRepository
{
    Task<Either<Problem, bool>> ExistUserWithEmail(string email, CancellationToken cancellationToken);
}