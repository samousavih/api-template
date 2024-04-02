using System;

namespace SampleOrg.WebAPI.User.Services.Responses;

public class GetUserResponse
{
    public UserDetails UserDetails { get; set; }
}

public class UserDetails
{
    public Guid UserId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public FinancialDetails FinancialDetails { get; set; }
}

public class FinancialDetails
{
    public decimal SalaryMonthly { get; set; }
    public decimal ExpensesMonthly { get; set; }
}