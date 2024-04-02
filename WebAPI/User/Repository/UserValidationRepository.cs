using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using LanguageExt;
using SampleOrg.WebAPI.Common;
using SampleOrg.WebAPI.User.Services.Validations;

namespace SampleOrg.WebAPI.User.Repository;

public class UserValidationRepository : IUserValidationRepository
{
    private readonly IDalConnection _dalConnection;

    public UserValidationRepository(IDalConnection dalConnection)
    {
        _dalConnection = dalConnection;
    }


    public async Task<Either<Problem, bool>> ExistUserWithEmail(string email, CancellationToken cancellationToken)
    {
        await using var connection = _dalConnection.Create();
        var parameters = new
        {
            Email = email
        };
        var commandDef = new CommandDefinition(
            $"SELECT count(*) FROM users WHERE email=@Email and status_code = '{UserStatusCode.Active.ToString()}';",
            parameters,
            cancellationToken: cancellationToken
        );

        var userCount = await connection.QueryAsync<decimal>(commandDef);
        return userCount.Single() > 0;
    }
}