namespace SleepingBearSystems.CraftingTools.Simulator;

public sealed record Job
{
    public Player Player { get; init; }

    public Recipe Recipe { get; init; }
}
