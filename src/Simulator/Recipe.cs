namespace SleepingBearSystems.CraftingTools.Simulator;

public sealed record Recipe
{
    public int Level { get; init; }

    public decimal Progress { get; init; }

    public decimal Quality { get; init; }

    public decimal Durability { get; init; }
}
