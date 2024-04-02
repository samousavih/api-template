using SampleOrg.WebAPI.User.Services.Responses;

namespace SampleOrg.WebAPI.User.Services.Mappers;

internal class UserResponseMapper : IUserResponseMapper
{
    public GetUserResponse Map(UserDetails userDetails)
    {
        return new GetUserResponse
        {
            UserDetails = new UserDetails
            {
                UserId = userDetails.UserId,
                Email = userDetails.Email,
                Name = userDetails.Name,
                FinancialDetails = new FinancialDetails
                {
                    SalaryMonthly = userDetails.FinancialDetails.SalaryMonthly,
                    ExpensesMonthly = userDetails.FinancialDetails.ExpensesMonthly
                }
            }
        };
    }
}