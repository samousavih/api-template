using System.Threading;
using System.Threading.Tasks;
using LanguageExt;
using MediatR;
using SampleOrg.WebAPI.Common;
using SampleOrg.WebAPI.User.Services.Mappers;
using SampleOrg.WebAPI.User.Services.Requests;
using SampleOrg.WebAPI.User.Services.Responses;
using SampleOrg.WebAPI.User.Services.Validations;

namespace SampleOrg.WebAPI.User.Services.Handlers;

public class FetchUserHandler : IRequestHandler<GetUserRequest, Either<Problem, GetUserResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly IUserRequestValidator _userRequestValidator;
    private readonly IUserResponseMapper _userResponseMapper;

    public FetchUserHandler(IUserRepository userRepository, IUserRequestValidator userRequestValidator,
        IUserResponseMapper userResponseMapper)
    {
        _userRepository = userRepository;
        _userRequestValidator = userRequestValidator;
        _userResponseMapper = userResponseMapper;
    }

    public Task<Either<Problem, GetUserResponse>> Handle(GetUserRequest request, CancellationToken cancellationToken)
    {
        return _userRequestValidator.Validate(request)
            .BindAsync(validatedGetUserRequest => _userRepository.FetchUser(validatedGetUserRequest, cancellationToken))
            .MapAsync(_userResponseMapper.Map);
    }
}