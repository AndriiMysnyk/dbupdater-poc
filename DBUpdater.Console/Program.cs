using FluentMigrator.Runner;
using DBUpdater.Common.SchemaLibrary;
using DBUpdater.Migrations;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using DBUpdater.Common;
using DBUpdater.Common.Migrations;

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

    private static ServiceProvider CreateServices()
    {
        IMigrationDescriptor migrationDescriptor = CreateMigrationDescriptor();
        ISchemaLibrary schemaLibrary = CreateSchemaLibrary();

        return new ServiceCollection()
            .AddFluentMigratorCore()
            .AddSingleton(migrationDescriptor)
            .AddSingleton(schemaLibrary)
            .ConfigureRunner(rb => rb
                .AddSQLite()
                .WithGlobalConnectionString(connectionString)
                .WithRunnerConventions(new DynamicMigrationRunnerConventions(migrationDescriptor))
                .ScanIn(typeof(DynamicMigration).Assembly).For.Migrations())
            .AddLogging(lb => lb.AddFluentMigratorConsole())
            .BuildServiceProvider(false);
    }

    private static IMigrationDescriptor CreateMigrationDescriptor()
    {
        MigrationDescriptor descriptor = new(
            migrationVersion,
            migrationDescription);

        return descriptor;
    }

    private static ISchemaLibrary CreateSchemaLibrary()
    {
        SchemaLibrary? data =
            JsonConvert.DeserializeObject<SchemaLibrary>(
                File.ReadAllText(schemaLibraryPath));
        
        SchemaLibrary schemaLibrary = new(data!.Tables);

        return schemaLibrary;
    }
}