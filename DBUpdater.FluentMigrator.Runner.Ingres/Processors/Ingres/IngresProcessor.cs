using System.Data;
using DBUpdater.FluentMigrator.Runner.Ingres;
using FluentMigrator.Expressions;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Helpers;
using FluentMigrator.Runner.Initialization;
using FluentMigrator.Runner.Processors;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DBUpdater.FluentMigrator.Runner.Processors.Ingres
{
    public class IngresProcessor : GenericProcessorBase
    {
        [CanBeNull]
        private readonly IServiceProvider _serviceProvider;
        [NotNull]
        private readonly IngresQuoter _quoter;

        public override string DatabaseType => "Ingres";

        public override IList<string> DatabaseTypeAliases { get; } = new List<string>();

        public IngresProcessor(
            [NotNull] IngresDbFactory factory,
            [NotNull] IngresGenerator generator,
            [NotNull] ILogger<IngresProcessor> logger,
            [NotNull] IOptionsSnapshot<ProcessorOptions> options,
            [NotNull] IConnectionStringAccessor connectionStringAccessor,
            [NotNull] IServiceProvider serviceProvider,
            [NotNull] IngresQuoter quoter)
            : base(() => factory.Factory, generator, logger, options.Value, connectionStringAccessor)
        {
            _serviceProvider = serviceProvider;
            _quoter = quoter;
        }

        public override void Process(PerformDBOperationExpression expression)
        {
            Logger.LogSay("Performing DB Operation");

            if (Options.PreviewOnly)
            {
                return;
            }

            EnsureConnectionIsOpen();

            expression.Operation?.Invoke(Connection, Transaction);
        }

        protected override void Process(string sql)
        {
            Logger.LogSql(sql);

            if (Options.PreviewOnly || string.IsNullOrEmpty(sql))
            {
                return;
            }

            EnsureConnectionIsOpen();

            using (var command = CreateCommand(sql))
            {
                command.ExecuteNonQuery();
            }
        }

        public override DataSet ReadTableData(string schemaName, string tableName)
        {
            return Read("select * from {0}", _quoter.QuoteTableName(tableName, schemaName));
        }

        public override DataSet Read(string template, params object[] args)
        {
            EnsureConnectionIsOpen();

            using (var command = CreateCommand(string.Format(template, args)))
            using (var reader = command.ExecuteReader())
            {
                return reader.ReadDataSet();
            }
        }

        public override bool Exists(string template, params object[] args)
        {
            EnsureConnectionIsOpen();

            using (var command = CreateCommand(string.Format(template, args)))
            {
                using (var reader = command.ExecuteReader())
                {
                    try
                    {
                        return reader.Read();
                    }
                    catch
                    {
                        return false;
                    }
                }
            }
        }

        public override void Execute(string template, params object[] args)
        {
            var commandText = string.Format(template, args);
            Logger.LogSql(commandText);

            if (Options.PreviewOnly)
            {
                return;
            }

            EnsureConnectionIsOpen();

            using (var command = CreateCommand(commandText))
            {
                command.ExecuteNonQuery();
            }
        }

        public override bool SchemaExists(string schemaName)
        {
            return Exists(
                "SELECT * FROM iischema WHERE schema_name = '{0}'",
                FormatHelper.FormatSqlEscape(schemaName));
        }

        public override bool TableExists(string schemaName, string tableName)
        {
            return Exists(
                "SELECT * FROM iitables WHERE table_name='{0}'",
                FormatHelper.FormatSqlEscape(tableName));
        }

        public override bool ColumnExists(string schemaName, string tableName, string columnName)
        {
            return Exists(
                //"SELECT * FROM iicolumns WHERE table_name='{0}' AND table_owner='{1}' AND column_name='{2}'",
                "SELECT * FROM iicolumns WHERE table_name='{0}' AND column_name='{1}'",
                FormatHelper.FormatSqlEscape(tableName),
                //FormatHelper.FormatSqlEscape(schemaName),
                FormatHelper.FormatSqlEscape(columnName));
        }

        public override bool ConstraintExists(string schemaName, string tableName, string constraintName)
        {
            return Exists(
                "SELECT * FROM iiconstraints WHERE table_name='{0}' AND table_owner='{1}' AND constraint_name='{2}'",
                FormatHelper.FormatSqlEscape(tableName),
                FormatHelper.FormatSqlEscape(schemaName),
                FormatHelper.FormatSqlEscape(constraintName));
        }

        public override bool IndexExists(string schemaName, string tableName, string indexName)
        {
            return Exists(
                "SELECT * FROM iiindexes WHERE base_name='{0}' AND index_owner='{1}' AND index_name='{2}'",
                FormatHelper.FormatSqlEscape(tableName),
                FormatHelper.FormatSqlEscape(schemaName),
                FormatHelper.FormatSqlEscape(indexName));
        }

        public override bool SequenceExists(string schemaName, string sequenceName)
        {
            return Exists(
                "SELECT * FROM iisequence WHERE seq_name='{0}' AND seq_owner='{1}'",
                FormatHelper.FormatSqlEscape(sequenceName),
                FormatHelper.FormatSqlEscape(schemaName));
        }

        public override bool DefaultValueExists(string schemaName, string tableName, string columnName, object defaultValue)
        {
            throw new NotImplementedException();
        }
    }
}
