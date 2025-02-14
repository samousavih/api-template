using System.Reflection;
using DbUp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SampleOrg.WebAPI.Common.Extensions;

public static class DatabaseExtension
{
    public static IHost MigrateDatabase<TContext>(this IHost host)
    {
        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var configuration = services.GetRequiredService<IConfiguration>();
            var logger = services.GetRequiredService<ILogger<TContext>>();

            logger.LogInformation("Migrating postresql database.");

            var connection = configuration.GetConnectionString("SampleOrgApi");

            EnsureDatabase.For.PostgresqlDatabase(connection);

            var upgrader = DeployChanges.To
                .PostgresqlDatabase(connection)
                .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                .LogToConsole()
                .Build();

            var result = upgrader.PerformUpgrade();

            if (!result.Successful)
            {
                logger.LogError(result.Error, "An error occurred while migrating the postresql database");
                return host;
            }

            logger.LogInformation("Migrated postresql database.");
        }

        return host;
    }
}