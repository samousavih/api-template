using System;
using System.Threading;
using System.Threading.Tasks;
using LanguageExt;
using SampleOrg.WebAPI.Common;

namespace SampleOrg.WebAPI.Account.Services.Validations;

public interface IAccountValidationRepository
{
    Task<Either<Problem, UserFinancialDetail>> FetchUserFinancialDetails(Guid userId,
        CancellationToken cancellationToken);
}