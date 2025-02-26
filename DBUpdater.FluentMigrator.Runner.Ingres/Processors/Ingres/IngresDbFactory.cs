using FluentMigrator.Runner.Processors;
using Ingres.Client;
using System.Data.Common;

namespace DBUpdater.FluentMigrator.Runner.Processors.Ingres
{
    public class IngresDbFactory : DbFactoryBase
    {
        public IngresDbFactory()
            : base(IngresFactory.Instance)
        {
        }

        protected override DbProviderFactory CreateFactory()
        {
            return IngresFactory.Instance;
        }
    }
}
