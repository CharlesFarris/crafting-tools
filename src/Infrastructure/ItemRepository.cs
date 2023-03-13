using System.Collections.Immutable;
using System.Diagnostics;
using SleepingBearSystems.CraftingTools.Application;
using SleepingBearSystems.CraftingTools.Domain;
using SleepingBearSystems.Tools.Common;

namespace SleepingBearSystems.CraftingTools.Infrastructure;

public sealed class ItemRepository : IRepository<Item>
{
    private readonly EventStore _eventStore;

    public ItemRepository(EventStore eventStore)
    {
        this._eventStore = eventStore ?? throw new ArgumentNullException(nameof(eventStore));
    }

    public async Task<Maybe<Item>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var streamName = StreamName.FromEntity<Item>(id);
        var events = await this._eventStore.GetEvents(streamName, cancellationToken);
        return events
            .Aggregate<IEvent?, Item?>(default, (current, @event) => @event switch
            {
                ItemCreatedEvent created => new Item(created.Id, created.Name),
                ItemRenamedEvent renamed => (current ?? throw new InvalidOperationException()) with
                {
                    Name = renamed.Name
                },
                _ => current
            })
            .ToMaybeClass();
    }

    public async Task<ImmutableList<Item>> GetAll(CancellationToken cancellationToken)
    {

        return ImmutableList<Item>.Empty;
    }
}
