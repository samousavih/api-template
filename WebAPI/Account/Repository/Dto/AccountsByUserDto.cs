using System;

namespace SampleOrg.WebAPI.Account.Repository.Dto;

public class AccountsByUserDto
{
    public Guid AccountId { get; set; }
    public Guid UserId { get; set; }
    public string AccountNumber { get; set; }
    public string Description { get; set; }
}