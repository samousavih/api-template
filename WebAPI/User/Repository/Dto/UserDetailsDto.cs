using System;

namespace SampleOrg.WebAPI.User.Repository.Dto;

public class UserDetailsDto
{
    public Guid UserId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public decimal SalaryMonthly { get; set; }
    public decimal ExpensesMonthly { get; set; }
}