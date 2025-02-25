using FluentMigrator.Runner.Processors;

namespace DBUpdater.FluentMigrator.Runner.Processors.Ingres
{
    public class IngresDbFactory : ReflectionBasedDbFactory
    {
        private static readonly TestEntry[] _testEntries = [ new("actian.client", "Ingres.Client.IngresFactory") ];

        [Obsolete]
        public IngresDbFactory()
            : base(_testEntries)
        {
        }

        public IngresDbFactory(IServiceProvider serviceProvider)
            : base(serviceProvider, _testEntries)
        {
        }
    }
}
