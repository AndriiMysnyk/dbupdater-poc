using FluentMigrator;
using DBUpdater.Common.SchemaLibrary;

namespace DBUpdater.Migrations;

[Migration(1)]
/// <summary>
/// We need only one <see cref="Migration"/> implementation whilch is completely ruled by <see cref="IMigrationConfig"/> data.
/// The core idea is to run the same migration with different input data (tables, constraints, etc.) and version info.
/// </summary>
public sealed class DynamicMigration : Migration
{
    private IMigrationConfig _config;

    public DynamicMigration(IMigrationConfig config)
    {
        _config = config;
    }

    public override void Up()
    {
        foreach (Table table in _config.Tables)
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
        foreach (Table table in _config.Tables)
        {
            Delete.Table(table.Name);
        }
    }
}
