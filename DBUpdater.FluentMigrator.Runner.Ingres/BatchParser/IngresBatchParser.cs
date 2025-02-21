using FluentMigrator.Runner.BatchParser.RangeSearchers;
using FluentMigrator.Runner.BatchParser.SpecialTokenSearchers;
using JetBrains.Annotations;

namespace FluentMigrator.Runner.BatchParser
{
    /// <summary>
    /// A specialization of the <see cref="SqlBatchParser"/> for the Ingres
    /// </summary>
    /// <remarks>
    /// It uses the following range searchers: <see cref="MultiLineComment"/>, <see cref="DoubleDashSingleLineComment"/>,
    /// <see cref="PoundSignSingleLineComment"/>, <see cref="AnsiSqlIdentifier"/>, <see cref="SqlString"/>
    /// and the following token searchers: <see cref="GoSearcher"/>.
    /// </remarks>
    public class IngresBatchParser : SqlBatchParser
    {
        [NotNull, ItemNotNull]
        private static readonly IEnumerable<IRangeSearcher> _rangeSearchers = new IRangeSearcher[]
        {
            new MultiLineComment(),
            new DoubleDashSingleLineComment(),
            new AnsiSqlIdentifier(),
            new SqlString(),
        };

        [NotNull, ItemNotNull]
        private static readonly IEnumerable<ISpecialTokenSearcher> _specialTokenSearchers = new ISpecialTokenSearcher[]
        {
            new GoSearcher(),
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="IngresBatchParser"/> class.
        /// </summary>
        /// <param name="newLine">The string used to write a new line sequence</param>
        public IngresBatchParser(string newLine = null)
            : base(_rangeSearchers, _specialTokenSearchers, newLine)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IngresBatchParser"/> class.
        /// </summary>
        /// <param name="rangeSearchers">The range searchers to be used</param>
        /// <param name="specialTokenSearchers">The special token searchers to be used</param>
        /// <param name="newLine">The new line sequence to be used for the output</param>
        protected IngresBatchParser(
            [NotNull, ItemNotNull] IEnumerable<IRangeSearcher> rangeSearchers,
            [NotNull, ItemNotNull] IEnumerable<ISpecialTokenSearcher> specialTokenSearchers,
            string newLine = null)
            : base(rangeSearchers, specialTokenSearchers, newLine)
        {
        }
    }
}
