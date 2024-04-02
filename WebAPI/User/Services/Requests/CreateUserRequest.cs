using LanguageExt;
using MediatR;
using SampleOrg.WebAPI.Common;
using SampleOrg.WebAPI.User.Services.Responses;

namespace SampleOrg.WebAPI.User.Services.Requests;

public class CreateUserRequest : IRequest<Either<Problem, CreateUserResponse>>
{
    public string Name { get; set; }
    public string Email { get; set; }
    public decimal SalaryMonthly { get; set; }
    public decimal ExpensesMonthly { get; set; }
}