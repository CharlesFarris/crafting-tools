using System.Collections.Immutable;
using SleepingBearSystems.CraftingTools.Domain;
using SleepingBearSystems.Tools.Common;
using SleepingBearSystems.Tools.Railway;

namespace SleepingBearSystems.CraftingTools.Application;

public sealed class ItemCommandHandler :
    ICommandHandler<CreateItemCommand>,
    ICommandHandler<RenameItemCommand>
{
    private readonly IEventStore _eventStore;

    public ItemCommandHandler(IEventStore eventStore)
    {
        this._eventStore = eventStore ?? throw new ArgumentNullException(nameof(eventStore));
    }

    public async Task<Result<Unit>> Handle(CreateItemCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var @event = new ItemCreatedEvent(command.Id, command.Name);
            await this._eventStore.AppendEvent(StreamName.FromEntity<Item>(command.Id), @event, cancellationToken);
            return Result<Unit>.Success(Unit.Default);
        }
        catch (Exception ex)
        {
            return Result<Unit>.Failure(ex.ToResultError("An error occurred."));
        }
    }

    public async Task<Result<Unit>> Handle(RenameItemCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var @event = new ItemRenamedEvent(command.Id, command.Name);
            await this._eventStore.AppendEvent(StreamName.FromEntity<Item>(command.Id), @event, cancellationToken);
            return Result<Unit>.Success(Unit.Default);
        }
        catch (Exception ex)
        {
            return Result<Unit>.Failure(ex.ToResultError("An error occurred."));
        }
    }
}
