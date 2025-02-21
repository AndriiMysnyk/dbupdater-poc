using FluentMigrator.Runner;
using DBUpdater.Common.SchemaLibrary;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using DBUpdater.Common;
using DBUpdater.Common.Migrations;
using DBUpdater.Console;

class Program
{
    private readonly static bool isIngres = false;
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

        IServiceCollection services = new ServiceCollection();
        services
            .AddFluentMigratorCore()
            .AddSingleton(migrationDescriptor)
            .AddSingleton(schemaLibrary)
            .AddLogging(lb => lb.AddFluentMigratorConsole());

        if (isIngres)
        {
            services.ConfigureIngresRunner(connectionString, migrationDescriptor);
        }
        else
        {
            services.ConfigureSQLiteRunner(connectionString, migrationDescriptor);
        }
            
        return services.BuildServiceProvider(false);
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