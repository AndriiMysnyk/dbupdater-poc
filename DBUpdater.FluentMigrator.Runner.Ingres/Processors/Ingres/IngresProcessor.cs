using System.Data;
using DBUpdater.FluentMigrator.Runner.Ingres;
using FluentMigrator;
using FluentMigrator.Expressions;
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
            throw new NotImplementedException();
        }

        protected override void Process(string sql)
        {
            throw new NotImplementedException();
        }

        public override DataSet ReadTableData(string schemaName, string tableName)
        {
            throw new NotImplementedException();
        }

        public override DataSet Read(string template, params object[] args)
        {
            throw new NotImplementedException();
        }

        public override bool Exists(string template, params object[] args)
        {
            throw new NotImplementedException();
        }

        public override void Execute(string template, params object[] args)
        {
            throw new NotImplementedException();
        }

        public override bool SchemaExists(string schemaName)
        {
            throw new NotImplementedException();
        }

        public override bool TableExists(string schemaName, string tableName)
        {
            throw new NotImplementedException();
        }

        public override bool ColumnExists(string schemaName, string tableName, string columnName)
        {
            throw new NotImplementedException();
        }

        public override bool ConstraintExists(string schemaName, string tableName, string constraintName)
        {
            throw new NotImplementedException();
        }

        public override bool IndexExists(string schemaName, string tableName, string indexName)
        {
            throw new NotImplementedException();
        }

        public override bool SequenceExists(string schemaName, string sequenceName)
        {
            throw new NotImplementedException();
        }

        public override bool DefaultValueExists(string schemaName, string tableName, string columnName, object defaultValue)
        {
            throw new NotImplementedException();
        }
    }
}
