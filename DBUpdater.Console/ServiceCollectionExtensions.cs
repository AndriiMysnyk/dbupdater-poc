using DBUpdater.Common;
using DBUpdater.FluentMigrator.Runner.Ingres;
using DBUpdater.Migrations;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;

namespace DBUpdater.Console;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureSQLiteRunner(
        this IServiceCollection services,
        string connectionString,
        IMigrationDescriptor migrationDescriptor)
    {
        return services
            .ConfigureRunner(rb => rb
                .AddSQLite()
                .WithGlobalConnectionString(connectionString)
                .WithRunnerConventions(new DynamicMigrationRunnerConventions(migrationDescriptor))
                .ScanIn(typeof(DynamicMigration).Assembly).For.Migrations());
    }

    public static IServiceCollection ConfigureIngresRunner(
        this IServiceCollection services,
        string connectionString,
        IMigrationDescriptor migrationDescriptor)
    {
        return services
            .ConfigureRunner(rb => rb
                .AddIngres()
                .WithGlobalConnectionString(connectionString)
                .WithRunnerConventions(new DynamicMigrationRunnerConventions(migrationDescriptor))
                .ScanIn(typeof(DynamicMigration).Assembly).For.Migrations());
    }
}
