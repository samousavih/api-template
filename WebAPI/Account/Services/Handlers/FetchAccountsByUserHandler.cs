using System.Threading;
using System.Threading.Tasks;
using LanguageExt;
using MediatR;
using SampleOrg.WebAPI.Account.Controllers.Responses;
using SampleOrg.WebAPI.Account.Services.Mappers;
using SampleOrg.WebAPI.Account.Services.Requests;
using SampleOrg.WebAPI.Account.Services.Validations;
using SampleOrg.WebAPI.Common;

namespace SampleOrg.WebAPI.Account.Services.Handlers;

public class
    FetchAccountsByUserHandler : IRequestHandler<FetchAccountsByUserRequest,
    Either<Problem, FetchAccountsByUserResponse>>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IAccountRequestValidator _accountRequestValidator;
    private readonly IAccountResponseMapper _accountResponseMapper;

    public FetchAccountsByUserHandler(IAccountRepository accountRepository,
        IAccountRequestValidator accountRequestValidator, IAccountResponseMapper accountResponseMapper)
    {
        _accountRepository = accountRepository;
        _accountRequestValidator = accountRequestValidator;
        _accountResponseMapper = accountResponseMapper;
    }

    public Task<Either<Problem, FetchAccountsByUserResponse>> Handle(FetchAccountsByUserRequest request,
        CancellationToken cancellationToken)
    {
        return _accountRequestValidator.Validate(request)
            .BindAsync(validatedFetchAccountByUserRequest =>
                _accountRepository.FetchAccount(validatedFetchAccountByUserRequest, cancellationToken))
            .MapAsync(_accountResponseMapper.Map);
    }
}