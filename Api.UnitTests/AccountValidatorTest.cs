using System;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using FluentAssertions;
using Moq;
using SampleOrg.Tests.TestHelper;
using SampleOrg.WebAPI.Account.Services.Requests;
using SampleOrg.WebAPI.Account.Services.Validations;
using SampleOrg.WebAPI.Common;
using Xunit;

namespace SampleOrg.Tests;

public class AccountValidatorTest
{
    [Theory]
    [AutoDomainData]
    public async Task Should_not_create_account_when_salary_expense_not_following_requirements(
        [Frozen] Mock<IAccountValidationRepository> accountValidationRepository,
        CreateAccountRequest createAccountRequest,
        AccountRequestValidator sut
    )
    {
        var userFinancialDetail = new UserFinancialDetail
        {
            UserId = createAccountRequest.UserId,
            SalaryMonthly = 100,
            ExpensesMonthly = 100
        };
        accountValidationRepository.Setup(x => x.FetchUserFinancialDetails(It.IsAny<Guid>(), default).Result)
            .Returns(userFinancialDetail);

        var result = await sut.Validate(createAccountRequest, default);

        result.IsLeft.Should().BeTrue();
        var problem = new Problem();
        result.Match(_ => { }, l => problem = l);
        problem.Type.Should().Be(ProblemType.NotEnoughSalary);
    }
}