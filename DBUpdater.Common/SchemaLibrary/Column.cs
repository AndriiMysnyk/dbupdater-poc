namespace DBUpdater.Common.SchemaLibrary;

public class Column
{
    public ColumnType Type { get; init; }

    public required string Name { get; init; }

    public string? Description { get; init; }

    public int? NumberOfPlaces { get; init; }

    public int? FractionalDigits { get; init; }

    public bool PartOfPrimaryKey { get; init; }

    public bool NotNull { get; init; }

    public bool Indexed { get; init; }

    //public ForeignKey ForeignKey { get; init; }

    public bool Visible { get; init; }

    //public ColumnSource Source { get; init; }

    public int Index { get; init; }
}
