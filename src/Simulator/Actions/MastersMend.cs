namespace SleepingBearSystems.CraftingTools.Simulator.Actions;

public sealed class MastersMend : JobAction
{
    public MastersMend()
        : base("Master's Mend", 7, 0)
    {
    }

    public override decimal SuccessRate(Job job, JobState jobState)
    {
        return 100;
    }

    public override JobState Execute(Job job, JobState jobState)
    {
        return new JobState()
        {
            Step = jobState.Step + 1,
            Actions = jobState.Actions.Add(this),
            CraftingPoints = jobState.CraftingPoints - 88,
            Progress = jobState.Progress,
            Quality = jobState.Quality,
            Durability = Math.Min(job.Recipe.Durability, jobState.Durability + 30)
        };
    }
}
