namespace SleepingBearSystems.CraftingTools.Simulator;

public abstract class JobAction
{
    protected JobAction(string name, int level)
    {
        this.Name = name;
        this.Level = level;
    }

    public abstract string Name { get; }

    public abstract int Level { get; }

    public abstract decimal SuccessRate(Job job, JobState jobState);

    public abstract JobState Execute(Job job, JobState jobState);

    public abstract decimal GetDurabilityCost(Job job, JobState jobState);

    public abstract int GetCraftingPointCost(Job job, JobState jobState);
}
