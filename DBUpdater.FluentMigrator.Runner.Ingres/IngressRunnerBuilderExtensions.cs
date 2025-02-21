using DBUpdater.FluentMigrator.Runner.Processors.Ingres;
using Microsoft.Extensions.DependencyInjection;
using FluentMigrator;
using FluentMigrator.Runner;

namespace DBUpdater.FluentMigrator.Runner.Ingres;

public static class IngressRunnerBuilderExtensions
{
    /// <summary>
    /// Adds Ingres support
    /// </summary>
    /// <param name="builder">The builder to add the Ingres-specific services to</param>
    /// <param name="binaryGuid">True if guids are stored as binary, false if guids are stored as string</param>
    /// <returns>The migration runner builder</returns>
    public static IMigrationRunnerBuilder AddIngres(this IMigrationRunnerBuilder builder, bool binaryGuid = false, bool useStrictTables = false)
    {
        builder.Services
            .AddScoped<IngresDbFactory>()
            .AddScoped<IngresQuoter>()
            .AddScoped<IngresProcessor>()
            .AddScoped<IMigrationProcessor>(sp => sp.GetRequiredService<IngresProcessor>())
            .AddScoped<IIngresTypeMap>(sp => new IngresTypeMap())
            .AddScoped<IngresGenerator>()
            .AddScoped<IMigrationGenerator>(sp => sp.GetRequiredService<IngresGenerator>());

        return builder;
    }
}
