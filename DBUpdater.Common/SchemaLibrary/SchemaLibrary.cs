namespace DBUpdater.Common.SchemaLibrary;

public class SchemaLibrary
{
    public SchemaLibrary(List<Table> tables)
    {
        this.Tables = tables;
    }

    public List<Table> Tables { get; init; }
}
