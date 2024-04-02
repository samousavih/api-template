using System.Threading.Tasks;
using AutoFixture.Xunit2;
using FluentAssertions;
using Moq;
using SampleOrg.Tests.TestHelper;
using SampleOrg.WebAPI.Common;
using SampleOrg.WebAPI.User.Services.Requests;
using SampleOrg.WebAPI.User.Services.Validations;
using Xunit;

namespace SampleOrg.Tests;

public class UserValidatorTest
{
    [Theory]
    [AutoDomainData]
    public async Task Should_not_pass_when_email_exists(
        [Frozen] Mock<IUserValidationRepository> userValidationRepository,
        CreateUserRequest createUserRequest,
        UserRequestValidator sut
    )
    {
        userValidationRepository.Setup(x => x.ExistUserWithEmail(It.IsAny<string>(), default).Result).Returns(true);

        var result = await sut.Validate(createUserRequest, default);

        result.IsLeft.Should().BeTrue();
        var problem = new Problem();
        result.Match(_ => { }, l => problem = l);
        problem.Type.Should().Be(ProblemType.UserExists);
    }
}