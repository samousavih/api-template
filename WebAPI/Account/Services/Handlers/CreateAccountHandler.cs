using System.Threading;
using System.Threading.Tasks;
using LanguageExt;
using MediatR;
using SampleOrg.WebAPI.Account.Services.Requests;
using SampleOrg.WebAPI.Account.Services.Responses;
using SampleOrg.WebAPI.Account.Services.Validations;
using SampleOrg.WebAPI.Common;

namespace SampleOrg.WebAPI.Account.Services.Handlers;

public class CreateAccountHandler : IRequestHandler<CreateAccountRequest, Either<Problem, CreateAccountResponse>>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IAccountRequestValidator _accountRequestValidator;

    public CreateAccountHandler(IAccountRepository accountRepository, IAccountRequestValidator accountRequestValidator)
    {
        _accountRepository = accountRepository;
        _accountRequestValidator = accountRequestValidator;
    }

    public Task<Either<Problem, CreateAccountResponse>> Handle(CreateAccountRequest request,
        CancellationToken cancellationToken)
    {
        return _accountRequestValidator.Validate(request, cancellationToken)
            .BindAsync(validatedCreateAccountRequest =>
                _accountRepository.CreateAccount(validatedCreateAccountRequest, cancellationToken))
            .MapAsync(accountId => new CreateAccountResponse
            {
                AccountId = accountId
            });
    }
}