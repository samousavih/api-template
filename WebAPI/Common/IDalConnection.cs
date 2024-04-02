using System.Data.Common;

namespace SampleOrg.WebAPI.Common;

public interface IDalConnection
{
    DbConnection Create();
}