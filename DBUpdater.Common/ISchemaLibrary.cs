using DBUpdater.Common.SchemaLibrary;

namespace DBUpdater.Common;

public interface ISchemaLibrary
{
    IEnumerable<Table> Tables { get; }
}
