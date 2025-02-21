namespace DBUpdater.Common.SchemaLibrary;

public class SchemaLibrary : ISchemaLibrary
{
    public SchemaLibrary(List<Table> tables) => Tables = tables;

    public List<Table> Tables { get; init; }
}
