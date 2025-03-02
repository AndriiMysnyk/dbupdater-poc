using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using DBUpdater.Migrations.Extensions;
using DBUpdater.Migrations.Enums;
using DBUpdater.Migrations.SchemaLibrary;
using DBUpdater.Migrations;

class Program
{
    #region Input parameters
    private readonly static bool isIngres = true;
    private readonly static string connectionString = @"Data Source=file:Databases\SQLite\Test.db";
    // The output file of MedicoTools.SchemaLibrary.Tester.exe (fd files data transformed into json)
    private readonly static string schemaLibraryPath = @"Input\lib.json";
    // Migration description & version are used for the migration history tracking
    private readonly static string migrationDescription = "First try to create tables";
    private readonly static long migrationVersion = 1;
    #endregion

    static void Main(string[] args)
    {
        using ServiceProvider serviceProvider = CreateServices();
        using IServiceScope scope = serviceProvider.CreateScope();

        IMigrationRunner runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();

        runner.MigrateUp();
    }

    private static ServiceProvider CreateServices()
    {
        IDatabaseUpdateDescriptor descriptor = CreateDescriptor();
        ISchemaLibrary library = CreateSchemaLibrary();

        IServiceCollection services = new ServiceCollection();
        services
            .AddDatabaseMigrator(
                isIngres ? DatabaseSystem.Ingres : DatabaseSystem.SqlLite,
                connectionString,
                descriptor,
                library);
            
        return services.BuildServiceProvider(false);
    }

    private static IDatabaseUpdateDescriptor CreateDescriptor()
    {
        DatabaseUpdateDescriptor descriptor = new(
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