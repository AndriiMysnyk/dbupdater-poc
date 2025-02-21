using DBUpdater.FluentMigrator.Runner.Processors.Ingres;
using Microsoft.Extensions.DependencyInjection;
using FluentMigrator;
using FluentMigrator.Runner;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using FluentMigrator.Runner.Generators;
using FluentMigrator.Runner.Processors;
using FluentMigrator.Runner.Initialization;

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
            .AddScoped<IngresProcessor>(sp =>
            {
                var factory = sp.GetService<IngresDbFactory>();
                var logger = sp.GetService<ILogger<IngresProcessor>>();
                var options = sp.GetService<IOptionsSnapshot<ProcessorOptions>>();
                var connectionStringAccessor = sp.GetService<IConnectionStringAccessor>();
                var quoter = new IngresQuoter();
                return new IngresProcessor(factory, sp.GetService<IngresGenerator>(), logger, options, connectionStringAccessor, sp, quoter);
            })
            .AddScoped<IIngresTypeMap>(sp => new IngresTypeMap())
            .AddScoped<IMigrationProcessor>(sp => sp.GetRequiredService<IngresProcessor>())

            .AddScoped(
                sp =>
                {
                    var typeMap = sp.GetRequiredService<IIngresTypeMap>();
                    return new IngresGenerator(
                        new IngresQuoter(),
                        typeMap,
                        new OptionsWrapper<GeneratorOptions>(new GeneratorOptions()));
                })
            .AddScoped<IMigrationGenerator>(sp => sp.GetRequiredService<IngresGenerator>());

        return builder;
    }
}
