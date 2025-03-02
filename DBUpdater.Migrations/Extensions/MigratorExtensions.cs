using DBUpdater.Migrations.SchemaLibrary;
using FluentMigrator.Builders.Create.Table;

namespace DBUpdater.Migrations.Extensions;

internal static class MigratorExtensions
{
    public static void ApplyType(this ICreateTableColumnAsTypeSyntax columnSyntax, Column column)
    {
        switch (column.Type)
        {
            case ColumnType.String:
                columnSyntax.AsString(column.NumberOfPlaces);
                break;
            case ColumnType.NString:
                columnSyntax.AsNString(column.NumberOfPlaces);
                break;
            case ColumnType.Integer:
                columnSyntax.AsInteger(column.NumberOfPlaces);
                break;
            case ColumnType.Date:
                columnSyntax.AsDate();
                break;
            case ColumnType.Timestamp:
                columnSyntax.AsTime();
                break;
            case ColumnType.Floating:
                columnSyntax.AsFloat(); // TODO: Check for column.NumberOfPlaces, column.FractionalDigits
                break;
            case ColumnType.Blob:
                columnSyntax.AsBlob(column.NumberOfPlaces);
                break;
            case ColumnType.CLob:
                columnSyntax.AsCLob(column.NumberOfPlaces);
                break;
            case ColumnType.NCLob:
                columnSyntax.AsNCLob(column.NumberOfPlaces);
                break;
            case ColumnType.Bit:
                columnSyntax.AsBoolean();
                break;
            case ColumnType.Seq:
                // TODO: Create a sequence with Migration's Create.Sequence syntax
                break;
            case ColumnType.Unknown:
                break;
        }
    }

    private static void AsString(this ICreateTableColumnAsTypeSyntax columnSyntax, int? numberOfPlaces)
    {
        if (numberOfPlaces is null)
        {
            columnSyntax.AsAnsiString();
        }
        else
        {
            columnSyntax.AsAnsiString(numberOfPlaces.Value);
        }
    }

    private static void AsCLob(this ICreateTableColumnAsTypeSyntax columnSyntax, int? numberOfPlaces) =>
        columnSyntax.AsString(numberOfPlaces);

    private static void AsNString(this ICreateTableColumnAsTypeSyntax columnSyntax, int? numberOfPlaces)
    {
        if (numberOfPlaces is null)
        {
            columnSyntax.AsString();
        }
        else
        {
            columnSyntax.AsString(numberOfPlaces.Value);
        }
    }

    private static void AsNCLob(this ICreateTableColumnAsTypeSyntax columnSyntax, int? numberOfPlaces) =>
        columnSyntax.AsNString(numberOfPlaces);

    private static void AsInteger(this ICreateTableColumnAsTypeSyntax columnSyntax, int? numberOfPlaces)
    {
        if (numberOfPlaces is null)
        {
            columnSyntax.AsInt64();
            return;
        }

        switch (numberOfPlaces.Value)
        {
            case 2:
                columnSyntax.AsInt16(); break;
            case 4:
                columnSyntax.AsInt32(); break;
            case 9:
                columnSyntax.AsInt64(); break;
        };
    }

    public static void AsBlob(this ICreateTableColumnAsTypeSyntax columnSyntax, int? numberOfPlaces)
    {
        if (numberOfPlaces is null)
        {
            columnSyntax.AsBinary();
        }
        else
        {
            columnSyntax.AsBinary(numberOfPlaces.Value);
        }
    }
}
