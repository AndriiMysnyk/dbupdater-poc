namespace DBUpdater.Migrations;

public interface IDatabaseUpdateDescriptor
{
    public long Version { get; }

    public string Description { get; }
}
