using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace SampleOrg.WebAPI.User.Controllers.Requests;

public class FetchUserRequest
{
    [Required] [FromQuery] public Guid UserId { get; set; }
}