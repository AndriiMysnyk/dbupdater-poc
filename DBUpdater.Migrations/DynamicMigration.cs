﻿using FluentMigrator;
using DBUpdater.Common.SchemaLibrary;
using DBUpdater.Common;
using DBUpdater.Migrations.Extensions;

namespace DBUpdater.Migrations;

[Migration(1)]
/// <summary>
/// We need only one <see cref="Migration"/> implementation whilch is completely ruled by <see cref="ISchemaLibrary"/> data.
/// The core idea is to run the same migration with different input data (tables, constraints, etc.) and version info.
/// </summary>
public sealed class DynamicMigration(ISchemaLibrary schemaLibrary) : Migration
{
    private readonly ISchemaLibrary _schemaLibrary = schemaLibrary;

    public override void Up()
    {
        foreach (Table table in _schemaLibrary.Tables)
        {
            if (Schema.Schema(table.Schema).Table(table.Name).Exists())
            {
                continue;
            }

            IfDatabase("ingers");

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
        //foreach (Table table in _schemaLibrary.Tables)
        //{
        //    Delete.Table(table.Name);
        //}
    }
}
