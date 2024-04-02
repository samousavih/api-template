using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace SampleOrg.WebAPI.Account.Controllers.Requests;

public class CreateAccountRequest
{
    [Required] [FromBody] public Guid UserId { get; set; }

    [Required] [FromBody] public string AccountNumber { get; set; }

    [Required] [FromBody] public string Description { get; set; }
}