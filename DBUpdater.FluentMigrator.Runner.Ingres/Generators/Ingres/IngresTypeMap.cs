using System.Data;
using FluentMigrator.Runner.Generators.Base;

namespace DBUpdater.FluentMigrator.Runner.Ingres;

public sealed class IngresTypeMap : TypeMapBase, IIngresTypeMap
{
    private const int DecimalCapacity = 1000;
    private const int MaxVarcharSize = 10485760;

    public IngresTypeMap()
    {
        SetupTypeMaps();
    }

    protected override void SetupTypeMaps()
    {
        SetTypeMap(DbType.String, "text");
        SetTypeMap(DbType.Byte, "integer");
        SetTypeMap(DbType.Int64, "integer");
        SetTypeMap(DbType.Date, "date");
        SetTypeMap(DbType.DateTime, "timestamp");
        SetTypeMap(DbType.DateTime2, "timestamp");
        SetTypeMap(DbType.String, "varchar($size)", MaxVarcharSize);
    }
}
