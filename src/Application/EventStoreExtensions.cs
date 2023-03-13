using System.Collections.Immutable;
using SleepingBearSystems.CraftingTools.Domain;
using SleepingBearSystems.Tools.Common;

namespace SleepingBearSystems.CraftingTools.Application;

public static class EventStoreExtensions
{
    public static async Task<Unit> AppendEvent(
        this IEventStore eventStore,
        string streamName,
        IEvent @event,
        CancellationToken cancellationToken = default)
    {
        return await eventStore.AppendEvents(streamName, ImmutableList<IEvent>.Empty.Add(@event), cancellationToken);
    }
}
