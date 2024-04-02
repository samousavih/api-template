using System;
using System.Threading;
using System.Threading.Tasks;
using LanguageExt;
using SampleOrg.WebAPI.Common;
using SampleOrg.WebAPI.User.Services.Responses;
using SampleOrg.WebAPI.User.Services.Validations;

namespace SampleOrg.WebAPI.User.Services.Handlers;

public interface IUserRepository
{
    Task<Either<Problem, UserDetails>> FetchUser(ValidatedGetUserRequest validatedGetUserRequest,
        CancellationToken cancellationToken);

    Task<Either<Problem, Guid>> CreateUser(ValidatedCreateUserRequest validatedCreateUserRequest,
        CancellationToken cancellationToken);
}