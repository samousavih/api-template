using System;
using System.Data.Common;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Dapper;
using FluentAssertions;
using Moq;
using Moq.Dapper;
using SampleOrg.Tests.TestHelper;
using SampleOrg.WebAPI.Common;
using SampleOrg.WebAPI.User.Repository;
using SampleOrg.WebAPI.User.Repository.Dto;
using SampleOrg.WebAPI.User.Services.Validations;
using Xunit;
using Responses_UserDetails = SampleOrg.WebAPI.User.Services.Responses.UserDetails;
using UserDetails = SampleOrg.WebAPI.User.Services.Responses.UserDetails;

namespace SampleOrg.Tests;

public class UserRepositoryTests
{
    [Theory]
    [AutoDomainData]
    public async Task Should_return_user(
        [Frozen] Mock<IDalConnection> mockDalConnection,
        [Frozen] Mock<DbConnection> mockDbConnection,
        ValidatedGetUserRequest validatedGetUserRequest,
        UserDetailsDto userDetailsDto,
        UserRepository sut
    )
    {
        mockDbConnection.SetupDapperAsync(dbConnection =>
                dbConnection.QuerySingleAsync<UserDetailsDto>(It.IsAny<CommandDefinition>()))
            .ReturnsAsync(userDetailsDto);

        mockDalConnection.Setup(x => x.Create())
            .Returns(mockDbConnection.Object);

        var result = await sut.FetchUser(
            validatedGetUserRequest,
            default
        );
        var userDetails = new Responses_UserDetails();
        result.IsRight.Should().BeTrue();
        result.Match(r => userDetails = r, _ => { });
        userDetails.UserId.Should().Be(userDetailsDto.UserId);
        userDetails.Name.Should().Be(userDetailsDto.Name);
        userDetails.Email.Should().Be(userDetailsDto.Email);
        userDetails.FinancialDetails.SalaryMonthly.Should().Be(userDetailsDto.SalaryMonthly);
        userDetails.FinancialDetails.ExpensesMonthly.Should().Be(userDetailsDto.ExpensesMonthly);
    }

    [Theory]
    [AutoDomainData]
    public async Task Should_return_not_found_problem(
        [Frozen] Mock<IDalConnection> mockDalConnection,
        [Frozen] Mock<DbConnection> mockDbConnection,
        ValidatedGetUserRequest validatedGetUserRequest,
        UserRepository sut
    )
    {
        mockDbConnection.SetupDapperAsync(dbConnection =>
                dbConnection.QueryAsync<UserDetailsDto>(It.IsAny<CommandDefinition>()))
            .ReturnsAsync(Array.Empty<UserDetailsDto>());

        mockDalConnection.Setup(x => x.Create())
            .Returns(mockDbConnection.Object);

        var result = await sut.FetchUser(
            validatedGetUserRequest,
            default
        );

        result.IsLeft.Should().BeTrue();
        var problem = new Problem();
        result.Match(_ => { }, l => { problem = l; });
        problem.Type.Should().Be(ProblemType.UserNotFound);
    }

    [Theory]
    [AutoDomainData]
    public async Task Should_create_user(
        [Frozen] Mock<IDalConnection> mockDalConnection,
        [Frozen] Mock<DbConnection> mockDbConnection,
        ValidatedCreateUserRequest validatedCreateUserRequest,
        UserRepository sut
    )
    {
        mockDbConnection.SetupDapperAsync(dbConnection =>
            dbConnection.ExecuteAsync(It.IsAny<CommandDefinition>()));

        mockDalConnection.Setup(x => x.Create())
            .Returns(mockDbConnection.Object);

        var result = await sut.CreateUser(
            validatedCreateUserRequest,
            default
        );
        var userId = new Guid();
        result.IsRight.Should().BeTrue();
        result.Match(r => userId = r, _ => { });
        userId.Should().NotBeEmpty();
    }
}