namespace DBUpdater.Common.SchemaLibrary;

public class Column
{
    public required string Name { get; init; }

    public string? Description { get; init; }

    public ColumnType Type { get; set; }
}
