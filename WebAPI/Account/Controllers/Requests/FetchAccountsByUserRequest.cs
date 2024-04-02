using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace SampleOrg.WebAPI.Account.Controllers.Requests;

public class FetchAccountsByUserRequest
{
    [Required] [FromQuery] public Guid UserId { get; set; }
}