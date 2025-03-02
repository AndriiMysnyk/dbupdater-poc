namespace DBUpdater.Migrations.SchemaLibrary;

public class Table
{
    public required string Schema { get; init; }

    public required string Name { get; init; }

    public required string Description { get; init; }

    public required List<Column> Columns { get; set; }
}
