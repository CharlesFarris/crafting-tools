namespace SleepingBearSystems.CraftingTools.Simulator;

public sealed record Player
{
    public int Level { get; init; }

    public int Craftsmanship { get; init; }

    public int Control { get; init; }

    public int CraftingPoints { get; init; }
}
