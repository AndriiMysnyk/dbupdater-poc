using FluentMigrator;
using DBUpdater.Common.SchemaLibrary;

namespace DBUpdater.Migrations;

[Migration(1)]
public class DynamicMigration : Migration
{
    private IMigrationConfig _config;

    public DynamicMigration(IMigrationConfig config) => _config = config;

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
                    .AsString();
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
