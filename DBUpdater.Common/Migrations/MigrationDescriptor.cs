namespace DBUpdater.Common.Migrations;

public class MigrationDescriptor(long version, string description)
    : IMigrationDescriptor
{
    public long Version { get; } = version;

    public string Description { get; } = description;
}
