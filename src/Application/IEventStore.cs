using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using SleepingBearSystems.CraftingTools.Domain;
using SleepingBearSystems.Tools.Common;
using SleepingBearSystems.Tools.Railway;

namespace SleepingBearSystems.CraftingTools.Application;

[SuppressMessage("Naming", "CA1716:Identifiers should not match keywords")]
public interface IEventStore
{
    Task<Result<Unit>> AppendEvents(string streamName, ImmutableList<IEvent> events, CancellationToken cancellationToken = default);

    Task<Result<ImmutableList<IEvent>>> GetEvents(string streamName, CancellationToken cancellationToken = default);

    void RegisterEvent<TEvent>() where TEvent : IEvent;
}
