using FluentMigrator.Runner;

namespace DBUpdater.Migrations;

internal class DatabaseMigrator : IDatabaseMigrator
{
    private readonly IMigrationRunner _migrationRunner;

    public DatabaseMigrator(IMigrationRunner migrationRunner)
    {
        _migrationRunner = migrationRunner;
    }

    public void Up()
    {
        _migrationRunner.MigrateUp();
    }
}
