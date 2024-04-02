using SampleOrg.WebAPI.User.Services.Responses;

namespace SampleOrg.WebAPI.User.Services.Mappers;

public interface IUserResponseMapper
{
    GetUserResponse Map(UserDetails userDetails);
}