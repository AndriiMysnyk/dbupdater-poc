using FluentMigrator.Infrastructure;
using FluentMigrator;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Infrastructure;
using System.Reflection;
using DBUpdater.Migrations;

namespace DBUpdater.Console;

/// <summary>
/// The only reason to override default conventions is to redefine migration version obtaining.
/// Default behavior is to get version value from <see cref="MigrationAttribute"/>
/// We need the ability to pass version value from the outside
/// </summary>
internal class DynamicMigrationRunnerConventions : IMigrationRunnerConventions
{
    private static readonly IMigrationRunnerConventions _default
        = DefaultMigrationRunnerConventions.Instance;

    public Func<Type, bool> TypeIsMigration { get; set; }
    public Func<Type, bool> TypeIsProfile { get; set; }
    public Func<Type, MigrationStage?> GetMaintenanceStage { get; set; }
    public Func<Type, bool> TypeIsVersionTableMetaData { get; set; }

    [Obsolete]
    public Func<Type, IMigrationInfo> GetMigrationInfo { get; set; }

    /// <inheritdoc />
    public Func<IMigration, IMigrationInfo> GetMigrationInfoForMigration => GetMigrationInfoForMigrationImpl;
    public Func<Type, bool> TypeHasTags { get; set; }
    public Func<Type, IEnumerable<string>, bool> TypeHasMatchingTags { get; set; }

    private IMigrationConfig _config;

    private IMigrationInfo GetMigrationInfoForMigrationImpl(IMigration migration)
    {
        var migrationType = migration.GetType();
        var migrationAttribute = migrationType.GetCustomAttribute<MigrationAttribute>();
        var migrationInfo = new MigrationInfo(
            _config.Version,
            _config.Description,
            migrationAttribute.TransactionBehavior,
            migrationAttribute.BreakingChange,
            () => migration);

        foreach (var traitAttribute in migrationType.GetCustomAttributes<MigrationTraitAttribute>(true))
            migrationInfo.AddTrait(traitAttribute.Name, traitAttribute.Value);

        return migrationInfo;
    }

    public DynamicMigrationRunnerConventions(IMigrationConfig config)
    {
        _config = config;
        TypeIsMigration = _default.TypeIsMigration;
        TypeIsVersionTableMetaData = _default.TypeIsVersionTableMetaData;
#pragma warning disable 612
        GetMigrationInfo = _default.GetMigrationInfo;
#pragma warning restore 612
        TypeIsProfile = _default.TypeIsProfile;
        GetMaintenanceStage = _default.GetMaintenanceStage;
        TypeHasTags = _default.TypeHasTags;
        TypeHasMatchingTags = _default.TypeHasMatchingTags;
    }
}
