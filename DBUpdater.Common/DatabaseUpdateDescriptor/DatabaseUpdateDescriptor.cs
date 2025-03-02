namespace DBUpdater.Common.Migrations;

public class DatabaseUpdateDescriptor(long version, string description)
    : IDatabaseUpdateDescriptor
{
    public long Version { get; } = version;

    public string Description { get; } = description;
}
