using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace SampleOrg.WebAPI.User.Controllers.Requests;

public class CreateUserRequest
{
    [Required] [FromBody] public string Name { get; set; }

    [Required] [FromBody] public string Email { get; set; }

    [Required] [FromBody] public decimal SalaryMonthly { get; set; }

    [Required] [FromBody] public decimal ExpensesMonthly { get; set; }
}