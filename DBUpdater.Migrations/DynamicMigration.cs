using FluentMigrator;
using DBUpdater.Common.SchemaLibrary;
using DBUpdater.Common;
using FluentMigrator.Builders.Create.Table;

namespace DBUpdater.Migrations;

[Migration(1)]
/// <summary>
/// We need only one <see cref="Migration"/> implementation whilch is completely ruled by <see cref="ISchemaLibrary"/> data.
/// The core idea is to run the same migration with different input data (tables, constraints, etc.) and version info.
/// </summary>
public sealed class DynamicMigration : Migration
{
    private ISchemaLibrary _schemaLibrary;

    public DynamicMigration(
        ISchemaLibrary schemaLibrary)
    {
        _schemaLibrary = schemaLibrary;
    }

    public override void Up()
    {
        foreach (Table table in _schemaLibrary.Tables)
        {
            if (Schema.Schema(table.Schema).Table(table.Name).Exists())
            {
                continue;
            }

            var createTableRequest =
                Create
                    .Table(table.Name)
                    .WithDescription(table.Description)
                    .InSchema(table.Schema);

            foreach (Column column in table.Columns)
            {
                var columnSyntax = createTableRequest
                    .WithColumn(column.Name);

                ApplyType(column, columnSyntax);
            }
        }
    }

    public override void Down()
    {
        foreach (Table table in _schemaLibrary.Tables)
        {
            Delete.Table(table.Name);
        }
    }


    private static void ApplyType(Column column, ICreateTableColumnAsTypeSyntax columnSyntax)
    {
        switch (column.Type)
        {
            case ColumnType.String:
                if (column.NumberOfPlaces is null)
                {
                    columnSyntax.AsAnsiString();
                }
                else
                {
                    columnSyntax.AsAnsiString(column.NumberOfPlaces.Value);
                }
                break;
            case ColumnType.NString:
                if (column.NumberOfPlaces is null)
                {
                    columnSyntax.AsString();
                }
                else
                {
                    columnSyntax.AsString(column.NumberOfPlaces.Value);
                }
                break;
            case ColumnType.Integer:
                columnSyntax.AsInt64();
                break;
            case ColumnType.Date:
                columnSyntax.AsDate();
                break;
            case ColumnType.Timestamp:
                columnSyntax.AsTime();
                break;
            case ColumnType.Floating:
                break;
            case ColumnType.Blob:
                break;
            case ColumnType.CLob:
                break;
            case ColumnType.NCLob:
                break;
            case ColumnType.Bit:
                break;
            case ColumnType.Seq:
                break;
            case ColumnType.Unknown:
                break;
        }
    }
}
