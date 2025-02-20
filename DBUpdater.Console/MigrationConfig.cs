using DBUpdater.Common.SchemaLibrary;
using DBUpdater.Migrations;

namespace DBUpdater.Console;

internal class MigrationConfig : IMigrationConfig
{
    public MigrationConfig(long version, string description, List<Table> tables)
    {
        Version = version;
        Description = description;
        Tables = tables;
    }

    public long Version { get; }

    public string Description { get; }

    public List<Table> Tables { get; }
}
