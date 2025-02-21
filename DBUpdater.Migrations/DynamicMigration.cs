using FluentMigrator;
using DBUpdater.Common.SchemaLibrary;
using DBUpdater.Common;

namespace DBUpdater.Migrations;

[Migration(1)]
/// <summary>
/// We need only one <see cref="Migration"/> implementation whilch is completely ruled by <see cref="ISchemaLibrary"/> data.
/// The core idea is to run the same migration with different input data (tables, constraints, etc.) and version info.
/// </summary>
public sealed class DynamicMigration : Migration
{
    private IMigrationDescriptor _descriptor;
    private ISchemaLibrary _schemaLibrary;

    public DynamicMigration(
        IMigrationDescriptor descriptor,
        ISchemaLibrary schemaLibrary)
    {
        _descriptor = descriptor;
        _schemaLibrary = schemaLibrary;
    }

    public override void Up()
    {
        foreach (Table table in _schemaLibrary.Tables)
        {
            var createTableRequest =
                Create
                    .Table(table.Name)
                    .WithDescription(table.Description)
                    .InSchema(table.Schema);

            foreach (Column column in table.Columns)
            {
                createTableRequest
                    .WithColumn(column.Name)
                    .AsString();                // TODO: Use Database dependent type translator
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
}
