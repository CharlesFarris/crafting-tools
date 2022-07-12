namespace SleepingBearSystems.CraftingTools.Simulator.Actions;

public sealed class StandardTouch : JobAction
{
    public StandardTouch()
        : base("Standard Touch", 18, 10)
    {
    }

    public override decimal SuccessRate(Job job, JobState jobState)
    {
        return 100;
    }

    public override JobState Execute(Job job, JobState jobState)
    {
        var cost = 32;
        if (jobState.Actions.LastOrDefault() is Observe)
        {
            cost = 18;
        }
        return new JobState()
        {
            Step = jobState.Step + 1,
            Actions = jobState.Actions.Add(this),
            CraftingPoints = jobState.CraftingPoints - cost,
            Progress = jobState.Progress,
            Quality = jobState.Quality,
            Durability = jobState.Durability - this.BaseDurabilityCost
        };
    }
}
