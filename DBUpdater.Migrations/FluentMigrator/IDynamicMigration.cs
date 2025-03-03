using FluentMigrator;

namespace DBUpdater.Migrations.FluentMigrator;

internal interface IDynamicMigration : IMigration
{
    public long Version { get; }

    public string Description { get; }
}
