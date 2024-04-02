using System;

namespace SampleOrg.WebAPI.Account.Services.Validations;

public class UserFinancialDetail
{
    public Guid UserId { get; set; }
    public decimal SalaryMonthly { get; set; }
    public decimal ExpensesMonthly { get; set; }
}