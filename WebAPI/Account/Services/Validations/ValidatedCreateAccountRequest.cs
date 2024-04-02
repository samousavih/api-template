using System;

namespace SampleOrg.WebAPI.Account.Services.Validations;

public class ValidatedCreateAccountRequest
{
    public Guid UserId { get; set; }
    public string AccountNumber { get; set; }
    public string Description { get; set; }
}