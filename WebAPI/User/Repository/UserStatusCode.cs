using System.Runtime.Serialization;

namespace SampleOrg.WebAPI.User.Repository;

public enum UserStatusCode
{
    [EnumMember(Value = "Active")] Active,
    [EnumMember(Value = "Deleted")] Deleted
}