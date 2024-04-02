using System.Runtime.Serialization;

namespace SampleOrg.WebAPI.Account.Repository;

public enum AccountStatusCode
{
    [EnumMember(Value = "Active")] Active,
    [EnumMember(Value = "Deleted")] Deleted
}