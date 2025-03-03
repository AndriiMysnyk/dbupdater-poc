using FluentMigrator.Runner.Initialization;

namespace DBUpdater.Migrations.FluentMigrator;

internal interface IDynamicMigrationSource : IFilteringMigrationSource
{
    void Add(IDynamicMigration migration);

    void Clear();
}
