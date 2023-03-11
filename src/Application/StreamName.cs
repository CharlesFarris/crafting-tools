using SleepingBearSystems.CraftingTools.Domain;

namespace SleepingBearSystems.CraftingTools.Application;

public static class StreamName
{
    public static string FromEntity<TEntity>(Guid id) where TEntity : IEntity
    {
        return $"{nameof(TEntity)}-{id}";
    }
}
