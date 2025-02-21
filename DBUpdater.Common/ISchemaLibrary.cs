using DBUpdater.Common.SchemaLibrary;

namespace DBUpdater.Common;

public interface ISchemaLibrary
{
    List<Table> Tables { get; }
}
