using DBUpdater.Migrations.FluentMigrator;
using DBUpdater.Migrations.SchemaLibrary;
using FluentMigrator.Runner;

namespace DBUpdater.Migrations;

internal class DatabaseMigrator : IDatabaseMigrator
{
    private readonly IMigrationRunner _migrationRunner;
    private readonly IDynamicMigrationSource _migrationSource;

    public DatabaseMigrator(
        IMigrationRunner migrationRunner,
        IDynamicMigrationSource migrationSource)
    {
        _migrationRunner = migrationRunner;
        _migrationSource = migrationSource;
    }

    public void Up(long version, string description, ISchemaLibrary library)
    {
        // TODO: Add logging
        DynamicMigration migration = new(version, description, library);
        _migrationSource.Add(migration);
        
        _migrationRunner.MigrateUp();

        _migrationSource.Clear();
    }
}
