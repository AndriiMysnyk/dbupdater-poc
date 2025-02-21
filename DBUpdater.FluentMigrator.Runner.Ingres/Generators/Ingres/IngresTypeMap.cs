using System.Data;
using FluentMigrator.Runner.Generators.Base;

namespace DBUpdater.FluentMigrator.Runner.Ingres;

public sealed class IngresTypeMap : TypeMapBase, IIngresTypeMap
{
    protected override void SetupTypeMaps()
    {
        SetTypeMap(DbType.String, "TEXT");
        SetTypeMap(DbType.Byte, "INTEGER");
    }
}
