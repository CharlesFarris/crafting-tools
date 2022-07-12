using System.Collections.Immutable;

namespace SleepingBearSystems.CraftingTools.Simulator;

public sealed record Rotation
{
    public decimal Quality { get; init; }

    public ImmutableList<JobAction> Actions { get; init; }
}
