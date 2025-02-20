using DBUpdater.Common.SchemaLibrary;

namespace DBUpdater.Migrations;

public interface IMigrationConfig
{
    public long Version { get; }

    public string Description { get; }

    List<Table> Tables { get; }
}
