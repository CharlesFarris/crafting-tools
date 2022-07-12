using System.Collections.Immutable;

namespace SleepingBearSystems.CraftingTools.Simulator;

public sealed record JobState
{
    public int Step { get; init; }

    public decimal Progress { get; init; }

    public decimal Quality { get; init; }

    public decimal Durability { get; init; }

    public Condition Condition { get; init; }

    public decimal CraftingPoints { get; init; }

    public ImmutableList<JobAction> Actions { get; init; }
}
