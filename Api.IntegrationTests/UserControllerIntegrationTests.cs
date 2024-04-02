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
using SampleOrg.WebAPI.User.Controllers.Responses;
using SampleOrg.WebAPI.User.Services.Handlers;
using SampleOrg.WebAPI.User.Services.Validations;
using Xunit;
using CreateUserRequest = SampleOrg.WebAPI.User.Controllers.Requests.CreateUserRequest;
using CreateUserResponse = SampleOrg.WebAPI.User.Controllers.Responses.CreateUserResponse;
using Requests_CreateUserRequest = SampleOrg.WebAPI.User.Controllers.Requests.CreateUserRequest;
using Responses_CreateUserResponse = SampleOrg.WebAPI.User.Controllers.Responses.CreateUserResponse;
using Responses_UserDetails = SampleOrg.WebAPI.User.Services.Responses.UserDetails;
using UserDetails = SampleOrg.WebAPI.User.Services.Responses.UserDetails;

namespace SampleOrg.Api.IntegrationTests;

public class UserControllerIntegrationTests : IClassFixture<IntegrationApplicationFactory<Startup>>
{
    private readonly WebApplicationFactory<Startup> _factory;

    public UserControllerIntegrationTests(IntegrationApplicationFactory<Startup> factory)
    {
        _factory = factory;
    }

    [Theory]
    [AutoDomainData]
    public async Task Should_fetch_user_by_id(
        Mock<IUserRepository> userRepository,
        Responses_UserDetails userDetails)
    {
        userRepository
            .Setup(x => x.FetchUser(It.IsAny<ValidatedGetUserRequest>(), It.IsAny<CancellationToken>()).Result)
            .Returns(userDetails);
        var testFactory = _factory.WithWebHostBuilder(
            config => config.ConfigureTestContainer<ContainerBuilder>(builder =>
            {
                builder
                    .RegisterMock(userRepository);
            }));

        var client = testFactory.CreateClient();

        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get,
            new Uri($"{client.BaseAddress}v1/users?userid={userDetails.UserId}"));
        httpRequestMessage.Headers.Add("Accept", "application/json");

        using var response = await client.SendAsync(httpRequestMessage);
        var content = await response.Content.ReadAsStringAsync();
        response.StatusCode.Should().Be(HttpStatusCode.OK, content);
        var fetchUserResponse = JsonConvert.DeserializeObject<FetchUserResponse>(content);
        fetchUserResponse.Should().NotBeNull();
        fetchUserResponse.UserDetails.Should().BeEquivalentTo(userDetails);
    }

    [Theory]
    [AutoDomainData]
    public async Task Should_create_user(
        Mock<IUserRepository> userRepository,
        Mock<IUserValidationRepository> userValidationRepository,
        Requests_CreateUserRequest createUserRequest,
        Guid userId)
    {
        userRepository
            .Setup(x => x.CreateUser(It.IsAny<ValidatedCreateUserRequest>(), It.IsAny<CancellationToken>()).Result)
            .Returns(userId);
        userValidationRepository
            .Setup(x => x.ExistUserWithEmail(It.IsAny<string>(), It.IsAny<CancellationToken>()).Result).Returns(false);
        var testFactory = _factory.WithWebHostBuilder(
            config => config.ConfigureTestContainer<ContainerBuilder>(builder =>
            {
                builder
                    .RegisterMock(userRepository)
                    .RegisterMock(userValidationRepository);
            }));

        var client = testFactory.CreateClient();

        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, new Uri($"{client.BaseAddress}v1/users"));
        httpRequestMessage.Headers.Add("Accept", "application/json");
        httpRequestMessage.Content = new StringContent(JsonConvert.SerializeObject(createUserRequest), Encoding.UTF8,
            "application/json");

        using var response = await client.SendAsync(httpRequestMessage);
        var content = await response.Content.ReadAsStringAsync();
        response.StatusCode.Should().Be(HttpStatusCode.OK, content);
        var createUserResponse = JsonConvert.DeserializeObject<Responses_CreateUserResponse>(content);
        createUserResponse.Should().NotBeNull();
        createUserResponse.UserId.Should().Be(userId);
    }
}