using DBUpdater.FluentMigrator.Runner.Ingres;
using DBUpdater.Migrations.Enums;
using DBUpdater.Migrations.FluentMigrator;
using DBUpdater.Migrations.SchemaLibrary;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;

namespace DBUpdater.Migrations.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDatabaseMigrator(
        this IServiceCollection services,
        DatabaseSystem dbSystem,
        string connectionString,
        IDatabaseUpdateDescriptor updateDescriptor,
        ISchemaLibrary schemaLibrary)
    {
        services
            .AddFluentMigratorCore()
            .AddLogging(lb => lb.AddFluentMigratorConsole());

        services
            .AddSingleton(updateDescriptor)
            .AddSingleton(schemaLibrary);

        return dbSystem switch
        {
            DatabaseSystem.SqlLite => services.ConfigureSQLiteRunner(connectionString, updateDescriptor),
            DatabaseSystem.Ingres => services.ConfigureIngresRunner(connectionString, updateDescriptor),
            _ => services,
        };
    }

    private static IServiceCollection ConfigureSQLiteRunner(
        this IServiceCollection services,
        string connectionString,
        IDatabaseUpdateDescriptor migrationDescriptor)
    {
        return services
            .ConfigureRunner(rb => rb
                .AddSQLite()
                .WithGlobalConnectionString(connectionString)
                .WithRunnerConventions(new DynamicMigrationRunnerConventions(migrationDescriptor))
                .ScanIn(typeof(DynamicMigration).Assembly).For.Migrations());
    }

    private static IServiceCollection ConfigureIngresRunner(
        this IServiceCollection services,
        string connectionString,
        IDatabaseUpdateDescriptor migrationDescriptor)
    {
        return services
            .ConfigureRunner(rb => rb
                .AddIngres()
                .WithGlobalConnectionString(connectionString)
                .WithRunnerConventions(new DynamicMigrationRunnerConventions(migrationDescriptor))
                .ScanIn(typeof(DynamicMigration).Assembly).For.Migrations());
    }
}
