using FluentMigrator.Runner;
using DBUpdater.Common.SchemaLibrary;
using DBUpdater.Migrations;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SqlKata;
using SqlKata.Compilers;
using SqlKata.Execution;

using DBUpdater.Console;

class Program
{
    static string connectionString = @$"Data Source=file:..\Databases\SQLite\Test.db ";

    private static MigrationConfig CreateConfig()
    {
        SchemaLibrary? data = JsonConvert.DeserializeObject<SchemaLibrary>(
            File.ReadAllText(@"..\Input\lib.json"));

        MigrationConfig config = new(4, "First try to create tables", data!.Tables);
        return config;
    }

    static void Main(string[] args)
    {
        using (var serviceProvider = CreateServices())
        using (var scope = serviceProvider.CreateScope())
        {
            UpdateDatabase(scope.ServiceProvider);
        }

        ReadTables();
    }

    private static void UpdateDatabase(IServiceProvider serviceProvider)
    {
        var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

        runner.MigrateUp();
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

    private static void ReadTables()
    {
        SqliteConnection connection = new(connectionString);

        QueryFactory factory = new QueryFactory(connection, new SqliteCompiler());

        Query query = factory
            .Query()
            .From("sqlite_master")
            .Where("type", "table");

        IEnumerable<dynamic> results = query.Get();

        foreach (dynamic result in results)
        {
            Console.WriteLine(result);
        }
    }
}