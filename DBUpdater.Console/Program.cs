using FluentMigrator.Runner;
using DBUpdater.Common.SchemaLibrary;
using DBUpdater.Migrations;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using DBUpdater.Console;

class Program
{
    private readonly static string connectionString = @"Data Source=file:..\Databases\SQLite\Test.db";
    private readonly static string schemaLibraryPath = @"..\Input\lib.json";
    private readonly static string migrationDescription = "First try to create tables";
    private readonly static long migrationVersion = 1;

    static void Main(string[] args)
    {
        using ServiceProvider serviceProvider = CreateServices();
        using IServiceScope scope = serviceProvider.CreateScope();

        IMigrationRunner runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();

        runner.MigrateUp();
    }

    private static MigrationConfig CreateConfig()
    {
        SchemaLibrary? data =
            JsonConvert.DeserializeObject<SchemaLibrary>(
                File.ReadAllText(schemaLibraryPath));

        MigrationConfig config = new(
            migrationVersion,
            migrationDescription,
            data!.Tables);

        return config;
    }

    private static ServiceProvider CreateServices()
    {
        IMigrationConfig config = CreateConfig();

        return new ServiceCollection()
            .AddFluentMigratorCore()
            .AddSingleton(config)
            .ConfigureRunner(rb => rb
                .AddSQLite()
                .WithGlobalConnectionString(connectionString)
                .WithRunnerConventions(new DynamicMigrationRunnerConventions(config))
                .ScanIn(typeof(DynamicMigration).Assembly).For.Migrations())
            .AddLogging(lb => lb.AddFluentMigratorConsole())
            .BuildServiceProvider(false);
    }
}