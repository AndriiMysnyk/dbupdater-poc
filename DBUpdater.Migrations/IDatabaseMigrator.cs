using DBUpdater.Migrations.SchemaLibrary;

namespace DBUpdater.Migrations;

public interface IDatabaseMigrator
{
    void Up(long version, string description, ISchemaLibrary library);
}
