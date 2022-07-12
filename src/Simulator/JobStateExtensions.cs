namespace SleepingBearSystems.CraftingTools.Simulator;

public static class JobStateExtensions
{
    public static bool IsValid(this JobState jobState)
    {
        if (jobState is null)
        {
            throw new ArgumentNullException(nameof(jobState));
        }

        if (jobState.Durability <= 0)
        {
            return false;
        }

        return true;
    }
}
