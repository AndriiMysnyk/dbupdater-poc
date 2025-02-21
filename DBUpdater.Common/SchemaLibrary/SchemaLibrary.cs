using System.Diagnostics.CodeAnalysis;

namespace DBUpdater.Common.SchemaLibrary;

public class SchemaLibrary : ISchemaLibrary
{
    public SchemaLibrary(IEnumerable<Table> tables) => Tables = tables ?? [];

    [NotNull]
    public IEnumerable<Table> Tables { get; init; }
}
