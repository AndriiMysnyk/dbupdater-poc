namespace DBUpdater.Migrations.SchemaLibrary;

public interface ISchemaLibrary
{
    IEnumerable<Table> Tables { get; }
}
