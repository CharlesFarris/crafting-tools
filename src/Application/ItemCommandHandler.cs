using SleepingBearSystems.CraftingTools.Domain;
using SleepingBearSystems.Tools.Common;
using SleepingBearSystems.Tools.Railway;

namespace SleepingBearSystems.CraftingTools.Application;

internal sealed class ItemCommandHandler :
    ICommandHandler<CreateItemCommand>,
    ICommandHandler<RenameItemCommand>
{
    private readonly IEventStore _eventStore;

    public ItemCommandHandler(IEventStore eventStore)
    {
        this._eventStore = eventStore ?? throw new ArgumentNullException(nameof(eventStore));
    }

    public async Task<Result<Unit>> Handle(CreateItemCommand command, CancellationToken cancellationToken = default)
    {
        var @event = new ItemCreatedEvent(command.Id, command.Name);
        return await this._eventStore.AppendEvent(StreamName.FromEntity<Item>(command.Id), @event, cancellationToken);
    }

    public async Task<Result<Unit>> Handle(RenameItemCommand command, CancellationToken cancellationToken = default)
    {
        var @event = new ItemRenamedEvent(command.Id, command.Name);
        return await this._eventStore.AppendEvent(StreamName.FromEntity<Item>(command.Id), @event, cancellationToken);
    }
}
