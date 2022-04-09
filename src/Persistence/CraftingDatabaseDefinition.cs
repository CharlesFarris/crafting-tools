using System.Collections.Immutable;
using SleepingBearSystems.Tools.Common;
using SleepingBearSystems.Tools.Persistence;
using SleepingBearSystems.Tools.Railway;

namespace SleepingBearSystems.CraftingTools.Persistence;

public static class CraftingDatabaseDefinition
{
    static CraftingDatabaseDefinition()
    {
        Definition = DatabaseDefinition.FromParameters(
                ImmutableList<UpgradeTask?>
                    .Empty
                    .Add(UpgradeTask
                        .FromParameters(
                            0,
                            new Guid("9B66EFCA-AE1A-49C2-9EA8-CFC040E7D3D6"),
                            "Create professions table",
                            typeof(CraftingDatabaseDefinition)
                                .GetStringEmbeddedResource("UpgradeTask_0_ProfessionsTable", true)
                                .Unwrap())
                        .Unwrap()),
                ImmutableArray<SynchronizationTask?>.Empty,
                "crafting_database_definition")
            .Unwrap();
    }

    public static DatabaseDefinition Definition { get; }
}
