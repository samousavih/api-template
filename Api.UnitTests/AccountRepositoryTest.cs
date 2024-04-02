using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Dapper;
using FluentAssertions;
using Moq;
using Moq.Dapper;
using SampleOrg.Tests.TestHelper;
using SampleOrg.WebAPI.Account.Repository;
using SampleOrg.WebAPI.Account.Repository.Dto;
using SampleOrg.WebAPI.Account.Services;
using SampleOrg.WebAPI.Account.Services.Validations;
using SampleOrg.WebAPI.Common;
using Xunit;

namespace SampleOrg.Tests;

public class AccountRepositoryTests
{
    [Theory]
    [AutoDomainData]
    public async Task Should_fetch_accounts_by_user([Frozen] Mock<IDalConnection> mockDalConnection,
        [Frozen] Mock<DbConnection> mockDbConnection,
        ValidatedFetchAccountByUserRequest validatedFetchAccountByUserRequest,
        AccountRepository sut,
        IEnumerable<AccountsByUserDto> accountsByUserDto)
    {
        mockDbConnection.SetupDapperAsync(dbConnection =>
                dbConnection.QueryAsync(It.IsAny<CommandDefinition>()))
            .ReturnsAsync(accountsByUserDto);
        ;

        mockDalConnection.Setup(x => x.Create())
            .Returns(mockDbConnection.Object);

        var result = await sut.FetchAccount(
            validatedFetchAccountByUserRequest,
            default
        );
        var userAccounts = new UserAccounts();
        result.IsRight.Should().BeTrue();
        result.Match(r => userAccounts = r, _ => { });
        userAccounts.UserId.Should().Be(userAccounts.UserId);
    }

    [Theory]
    [AutoDomainData]
    public async Task Should_create_account(
        [Frozen] Mock<IDalConnection> mockDalConnection,
        [Frozen] Mock<DbConnection> mockDbConnection,
        ValidatedCreateAccountRequest validatedCreateAccountRequest,
        AccountRepository sut
    )
    {
        mockDbConnection.SetupDapperAsync(dbConnection =>
            dbConnection.ExecuteAsync(It.IsAny<CommandDefinition>()));

        mockDalConnection.Setup(x => x.Create())
            .Returns(mockDbConnection.Object);

        var result = await sut.CreateAccount(
            validatedCreateAccountRequest,
            default
        );
        var accountId = new Guid();
        result.IsRight.Should().BeTrue();
        result.Match(r => accountId = r, _ => { });
        accountId.Should().NotBeEmpty();
    }
}