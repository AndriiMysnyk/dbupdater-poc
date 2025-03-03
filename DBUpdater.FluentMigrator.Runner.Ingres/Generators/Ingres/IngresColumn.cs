using FluentMigrator.Model;
using FluentMigrator.Runner.Generators;
using FluentMigrator.Runner.Generators.Base;

namespace DBUpdater.FluentMigrator.Runner.Generators.Ingres;

internal class IngresColumn : ColumnBase<IIngresTypeMap>
{
    public IngresColumn(IIngresTypeMap typeMap, IQuoter quoter)
        : base(typeMap, quoter)
    {
    }

    protected override string FormatIdentity(ColumnDefinition column)
    {
        return string.Empty;
    }
}
