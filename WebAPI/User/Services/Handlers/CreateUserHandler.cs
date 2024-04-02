using System.Threading;
using System.Threading.Tasks;
using LanguageExt;
using MediatR;
using SampleOrg.WebAPI.Common;
using SampleOrg.WebAPI.User.Services.Requests;
using SampleOrg.WebAPI.User.Services.Responses;
using SampleOrg.WebAPI.User.Services.Validations;

namespace SampleOrg.WebAPI.User.Services.Handlers;

public class CreateUserHandler : IRequestHandler<CreateUserRequest, Either<Problem, CreateUserResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly IUserRequestValidator _userRequestValidator;

    public CreateUserHandler(IUserRepository userRepository, IUserRequestValidator userRequestValidator)
    {
        _userRepository = userRepository;
        _userRequestValidator = userRequestValidator;
    }

    public Task<Either<Problem, CreateUserResponse>> Handle(CreateUserRequest request,
        CancellationToken cancellationToken)
    {
        return _userRequestValidator.Validate(request, cancellationToken)
            .BindAsync(validatedCreateUserRequest =>
                _userRepository.CreateUser(validatedCreateUserRequest, cancellationToken))
            .MapAsync(userId => new CreateUserResponse
            {
                UserId = userId
            });
    }
}