using System.Diagnostics.CodeAnalysis;
using FluentMigrator.Expressions;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Generators;
using FluentMigrator.Runner.Generators.Generic;

using Microsoft.Extensions.Options;

namespace DBUpdater.FluentMigrator.Runner.Ingres
{
    public class IngresGenerator : GenericGenerator
    {
        public IngresGenerator()
            : this(new IngresQuoter())
        {
        }

        public IngresGenerator(
            [NotNull] IngresQuoter quoter)
            : this(quoter, new OptionsWrapper<GeneratorOptions>(new GeneratorOptions()))
        {
        }

        public IngresGenerator(
            [NotNull] IngresQuoter quoter,
            [NotNull] IIngresTypeMap typeMap)
            : this(quoter, typeMap, new OptionsWrapper<GeneratorOptions>(new GeneratorOptions()))
        {
            
        }

        public IngresGenerator(
            [NotNull] IngresQuoter quoter,
            [NotNull] IOptions<GeneratorOptions> generatorOptions)
            : this(quoter, new IngresTypeMap(), generatorOptions)
        {
        }

        public IngresGenerator(
            [NotNull] IngresQuoter quoter,
            [NotNull] IIngresTypeMap typeMap,
            [NotNull] IOptions<GeneratorOptions> generatorOptions)
            : base(new IngresColumn(typeMap, quoter), quoter, new EmptyDescriptionGenerator(), generatorOptions)
        {
            CompatibilityMode = generatorOptions.Value.CompatibilityMode ?? CompatibilityMode.STRICT;
        }

        public override string Generate(AlterDefaultConstraintExpression expression)
        {
            throw new NotImplementedException();
        }

        public override string Generate(DeleteDefaultConstraintExpression expression)
        {
            throw new NotImplementedException();
        }
    }
}
