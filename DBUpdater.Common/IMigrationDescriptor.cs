namespace DBUpdater.Common;

public interface IMigrationDescriptor
{
    public long Version { get; }

    public string Description { get; }
}
