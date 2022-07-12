namespace SleepingBearSystems.CraftingTools.Simulator.Actions;

public sealed class BasicSynthesis : JobAction
{
    public BasicSynthesis()
        : base("Basic Synthesis", 1, 10)
    {
    }

    public override decimal GetDurabilityCost(Job job, JobState jobState)
    {
        return 10;
    }

    public override int GetCraftingPointCost(Job job, JobState jobState)
    {
        return 0;
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
            CraftingPoints = jobState.CraftingPoints - this.GetCraftingPointCost(job, jobState),
            Progress = jobState.Progress + 1,
            Quality = jobState.Quality,
            Durability = jobState.Durability - this.GetDurabilityCost(job, jobState)
        };
    }
}
