using DBUpdater.FluentMigrator.Runner.Ingres.Extensions;
using DBUpdater.Migrations.Enums;
using DBUpdater.Migrations.FluentMigrator;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Initialization;
using Microsoft.Extensions.DependencyInjection;

namespace DBUpdater.Migrations.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDatabaseMigrator(
        this IServiceCollection services,
        DatabaseSystem dbSystem,
        string connectionString)
    {
        services
            .AddFluentMigratorCore()
            .AddLogging(lb => lb.AddFluentMigratorConsole());

        DynamicMigrationSource dynamicMigrationSource = new();
        services
            .AddScoped<IDatabaseMigrator, DatabaseMigrator>()
            .AddScoped<DynamicMigrationSource>()
            .AddScoped<IDynamicMigrationSource>(sp => sp.GetRequiredService<DynamicMigrationSource>());

        return dbSystem switch
        {
            DatabaseSystem.SqlLite => services.ConfigureSQLiteRunner(connectionString),
            DatabaseSystem.Ingres => services.ConfigureIngresRunner(connectionString),
            _ => services,
        };
    }

    private static IServiceCollection ConfigureSQLiteRunner(
        this IServiceCollection services,
        string connectionString)
    {
        return services
            .ConfigureRunner(rb => rb
                .AddSQLite()
                .WithGlobalConnectionString(connectionString)
                .WithRunnerConventions(new DynamicMigrationRunnerConventions())
                .WithDynamicMigrationSource());
    }

    private static IServiceCollection ConfigureIngresRunner(
        this IServiceCollection services,
        string connectionString)
    {
        return services
            .ConfigureRunner(rb => rb
                .AddIngres()
                .WithGlobalConnectionString(connectionString)
                .WithRunnerConventions(new DynamicMigrationRunnerConventions())
                .WithDynamicMigrationSource());
    }

    private static IMigrationRunnerBuilder WithDynamicMigrationSource(
        this IMigrationRunnerBuilder builder)
    {
        builder.Services
            .AddScoped<IFilteringMigrationSource>(sp => sp.GetRequiredService<DynamicMigrationSource>())
#pragma warning disable 618
            .AddScoped<IMigrationSource>(sp => sp.GetRequiredService<DynamicMigrationSource>());
#pragma warning restore 618;
        return builder;
    }
}
