using System.Data.Common;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace SampleOrg.WebAPI.Common;

public class DalConnection : IDalConnection
{
    private readonly IConfiguration _configuration;

    public DalConnection(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public DbConnection Create()
    {
        return new NpgsqlConnection(GetConnectionString());
    }

    private string GetConnectionString()
    {
        return _configuration.GetConnectionString("SampleOrgApi");
    }
}