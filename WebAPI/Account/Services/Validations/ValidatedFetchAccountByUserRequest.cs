using System;

namespace SampleOrg.WebAPI.Account.Services.Validations;

public class ValidatedFetchAccountByUserRequest
{
    public Guid UserId { get; set; }
}