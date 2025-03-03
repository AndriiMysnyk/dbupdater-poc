using FluentMigrator;

namespace DBUpdater.Migrations.FluentMigrator;

internal class DynamicMigrationSource : IDynamicMigrationSource
{
    private readonly List<IDynamicMigration> _migrations = [];

    public void Add(IDynamicMigration migration) => _migrations.Add(migration);

    public void Clear() => _migrations.Clear();

    public IEnumerable<IMigration> GetMigrations(Func<Type, bool> predicate) => _migrations;

    public IEnumerable<IMigration> GetMigrations() => _migrations;
}
