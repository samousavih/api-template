using System.Net;
using System.Text;
using Autofac;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Moq;
using Newtonsoft.Json;
using SampleOrg.Api.IntegrationTests.TestHelper;
using SampleOrg.WebAPI;
using SampleOrg.WebAPI.Account.Controllers.Responses;
using SampleOrg.WebAPI.Account.Services;
using SampleOrg.WebAPI.Account.Services.Handlers;
using SampleOrg.WebAPI.Account.Services.Validations;
using Xunit;
using CreateAccountRequest = SampleOrg.WebAPI.Account.Controllers.Requests.CreateAccountRequest;
using CreateAccountResponse = SampleOrg.WebAPI.Account.Services.Responses.CreateAccountResponse;
using FetchAccountsByUserRequest = SampleOrg.WebAPI.Account.Controllers.Requests.FetchAccountsByUserRequest;
using Requests_CreateAccountRequest = SampleOrg.WebAPI.Account.Controllers.Requests.CreateAccountRequest;
using Requests_FetchAccountsByUserRequest = SampleOrg.WebAPI.Account.Controllers.Requests.FetchAccountsByUserRequest;
using Responses_CreateAccountResponse = SampleOrg.WebAPI.Account.Services.Responses.CreateAccountResponse;

namespace SampleOrg.Api.IntegrationTests;

public class AccountControllerIntegrationTests : IClassFixture<IntegrationApplicationFactory<Startup>>
{
    private readonly WebApplicationFactory<Startup> _factory;

    public AccountControllerIntegrationTests(IntegrationApplicationFactory<Startup> factory)
    {
        _factory = factory;
    }

    [Theory]
    [AutoDomainData]
    public async Task Should_fetch_account_by_user_id(
        Mock<IAccountRepository> accountRepository,
        Requests_FetchAccountsByUserRequest fetchAccountsByUserRequest,
        UserAccounts userAccounts)
    {
        accountRepository
            .Setup(x => x.FetchAccount(It.IsAny<ValidatedFetchAccountByUserRequest>(), It.IsAny<CancellationToken>())
                .Result).Returns(userAccounts);
        var testFactory = _factory.WithWebHostBuilder(
            config => config.ConfigureTestContainer<ContainerBuilder>(builder =>
            {
                builder
                    .RegisterMock(accountRepository);
            }));

        var client = testFactory.CreateClient();

        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get,
            new Uri($"{client.BaseAddress}v1/accounts?userid={userAccounts.UserId}"));
        httpRequestMessage.Headers.Add("Accept", "application/json");

        using var response = await client.SendAsync(httpRequestMessage);
        var content = await response.Content.ReadAsStringAsync();
        response.StatusCode.Should().Be(HttpStatusCode.OK, content);
        var fetchAccountsByUserResponse = JsonConvert.DeserializeObject<FetchAccountsByUserResponse>(content);
        fetchAccountsByUserResponse.Should().NotBeNull();
        fetchAccountsByUserResponse.AccountDetails.Count().Should().Be(userAccounts.Accounts.Count());
    }

    [Theory]
    [AutoDomainData]
    public async Task Should_create_account(
        Mock<IAccountRepository> accountRepository,
        Mock<IAccountValidationRepository> accountValidationRepository,
        Requests_CreateAccountRequest createAccountRequest,
        Guid accountId)
    {
        var userFinancialDetail = new UserFinancialDetail
        {
            UserId = createAccountRequest.UserId,
            SalaryMonthly = decimal.MaxValue,
            ExpensesMonthly = 0
        };
        accountRepository
            .Setup(x => x.CreateAccount(It.IsAny<ValidatedCreateAccountRequest>(), It.IsAny<CancellationToken>())
                .Result).Returns(accountId);
        accountValidationRepository
            .Setup(x => x.FetchUserFinancialDetails(It.IsAny<Guid>(), It.IsAny<CancellationToken>()).Result)
            .Returns(userFinancialDetail);
        var testFactory = _factory.WithWebHostBuilder(
            config => config.ConfigureTestContainer<ContainerBuilder>(builder =>
            {
                builder
                    .RegisterMock(accountRepository)
                    .RegisterMock(accountValidationRepository);
            }));

        var client = testFactory.CreateClient();

        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, new Uri($"{client.BaseAddress}v1/accounts"));
        httpRequestMessage.Headers.Add("Accept", "application/json");
        httpRequestMessage.Content = new StringContent(JsonConvert.SerializeObject(createAccountRequest), Encoding.UTF8,
            "application/json");

        using var response = await client.SendAsync(httpRequestMessage);
        var content = await response.Content.ReadAsStringAsync();
        response.StatusCode.Should().Be(HttpStatusCode.OK, content);
        var createAccountResponse = JsonConvert.DeserializeObject<Responses_CreateAccountResponse>(content);
        createAccountResponse.Should().NotBeNull();
        createAccountResponse.AccountId.Should().Be(accountId);
    }
}