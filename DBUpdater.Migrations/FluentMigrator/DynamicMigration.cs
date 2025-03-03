using FluentMigrator;
using DBUpdater.Migrations.Extensions;
using DBUpdater.Migrations.SchemaLibrary;

namespace DBUpdater.Migrations.FluentMigrator;

[Migration(1)]
/// <summary>
/// We need only one <see cref="Migration"/> implementation whilch is completely ruled by <see cref="ISchemaLibrary"/> data.
/// The core idea is to run the same migration with different input data (tables, constraints, etc.) and version info.
/// </summary>
public sealed class DynamicMigration(long version, string description, ISchemaLibrary schemaLibrary)
    : Migration, IDynamicMigration
{
    private readonly ISchemaLibrary _schemaLibrary = schemaLibrary;

    public long Version => version;

    public string Description => description;

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
                createTableRequest
                    .WithColumn(column.Name)
                    .ApplyType(column);
            }
        }
    }

    public override void Down()
    {
        // Not implemented
    }
}
