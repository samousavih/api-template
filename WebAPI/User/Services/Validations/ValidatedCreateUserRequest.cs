namespace SampleOrg.WebAPI.User.Services.Validations;

public class ValidatedCreateUserRequest
{
    public string Name { get; set; }

    public string Email { get; set; }

    public decimal SalaryMonthly { get; set; }

    public decimal ExpensesMonthly { get; set; }
}