namespace SleepingBearSystems.CraftingTools.Simulator;

public sealed record JobStep
{
    public JobState JobState { get; init; }

    public JobAction JobAction { get; init; }
}
