using System.Data;
using FluentMigrator.Runner.Generators.Base;

namespace DBUpdater.FluentMigrator.Runner.Generators.Ingres;

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
        SetTypeMap(DbType.Byte, "tinyint");
        SetTypeMap(DbType.Int16, "smallint");
        SetTypeMap(DbType.Int32, "integer");
        SetTypeMap(DbType.Int64, "bigint");
        SetTypeMap(DbType.Date, "date");
        SetTypeMap(DbType.Time, "time");
        SetTypeMap(DbType.DateTime, "timestamp");
        SetTypeMap(DbType.DateTime2, "timestamp");
        SetTypeMap(DbType.String, "text");
        SetTypeMap(DbType.String, "nvarchar($size)", MaxVarcharSize);
        SetTypeMap(DbType.AnsiString, "varchar($size)", MaxVarcharSize);
    }
}
