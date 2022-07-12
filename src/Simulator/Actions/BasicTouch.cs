namespace SleepingBearSystems.CraftingTools.Simulator.Actions;

public sealed class BasicTouch : JobAction
{
    public BasicTouch()
        : base("Basic Touch", 5, 10)
    {
    }

    public override decimal SuccessRate(Job job, JobState jobState)
    {
        if (jobState.CraftingPoints < 18)
        {
            return 0;
        }
        return 100;
    }

    public override JobState Execute(Job job, JobState jobState)
    {
        return new JobState()
        {
            Step = jobState.Step + 1,
            Actions = jobState.Actions.Add(this),
            CraftingPoints = jobState.CraftingPoints - 18,
            Progress = jobState.Progress,
            Quality = jobState.Quality + 1,
            Durability = jobState.Durability - this.BaseDurabilityCost,
        };
    }
}
