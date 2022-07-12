namespace SleepingBearSystems.CraftingTools.Simulator.Actions;

public sealed class Veneration : JobAction
{
    public Veneration()
        : base("Veneration", 15, 10)
    {
    }

    public override decimal SuccessRate(Job job, JobState jobState)
    {
        return 100;
    }

    public override JobState Execute(Job job, JobState jobState)
    {
        return new JobState
        {
            Step = jobState.Step + 1,
            Actions = jobState.Actions.Add(this),
            CraftingPoints = jobState.CraftingPoints - 18,
            Progress = jobState.Progress,
            Quality = jobState.Quality,
            Durability = jobState.Durability - this.BaseDurabilityCost
        };
    }
}
