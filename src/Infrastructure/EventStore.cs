using System.Collections.Immutable;
using System.Text;
using System.Text.Json;
using EventStore.Client;
using SleepingBearSystems.CraftingTools.Application;
using SleepingBearSystems.CraftingTools.Domain;
using SleepingBearSystems.Tools.Common;
using SleepingBearSystems.Tools.Railway;

namespace SleepingBearSystems.CraftingTools.Infrastructure;

public sealed class EventStore :IEventStore
{
    private readonly EventStoreClient _client;

    private ImmutableDictionary<string, Type> _eventTypeMap = ImmutableDictionary<string, Type>.Empty;

    public EventStore(EventStoreClient client)
    {
        this._client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async Task<Result<Unit>> AppendEvents(string streamName, ImmutableList<IEvent> events, CancellationToken cancellationToken = default)
    {
        try
        {
            var eventData = events.Select(@event =>
            {
                var json = JsonSerializer.Serialize(@event);
                var data = Encoding.UTF8.GetBytes(json);
                return new EventData(
                    Uuid.NewUuid(),
                    @event.GetType().Name,
                    data);
            });
            await this._client.AppendToStreamAsync(
                streamName,
                StreamRevision.None,
                eventData,
                cancellationToken: cancellationToken);
            return Result<Unit>.Success(Unit.Default);
        }
        catch (Exception ex)
        {
            return Result<Unit>.Failure(ex.ToResultError("An error occurred appending events."));
        }
    }

    public async Task<Result<ImmutableList<IEvent>>> GetEvents(string streamName, CancellationToken cancellationToken)
    {
        try
        {
            var readStreamResult = this._client.ReadStreamAsync(
                Direction.Forwards,
                streamName,
                StreamPosition.Start,
                cancellationToken: cancellationToken);
            var events = ImmutableList<IEvent>.Empty;
            await foreach (var resolvedEvent in readStreamResult)
            {
                var json = Encoding.UTF8.GetString(resolvedEvent.Event.Data.Span);

                if (!this._eventTypeMap.TryGetValue(resolvedEvent.Event.EventType, out var eventType))
                {
                    throw new InvalidOperationException($"Unknown event type: {resolvedEvent.Event.EventType}.");
                }

                var obj = JsonSerializer.Deserialize(json, eventType);
                events = obj is IEvent @event
                    ? events.Add(@event)
                    : throw new InvalidOperationException($"Invalid event type: {eventType.Name}");
            }
            return Result<ImmutableList<IEvent>>.Success(events);
        }
        catch (Exception ex)
        {
            return Result<ImmutableList<IEvent>>.Failure(ex.ToResultError("An error occurred reading events."));
        }
    }

    public void RegisterEvent<TEvent>() where TEvent : IEvent
    {
        var eventType = typeof(TEvent);
        if (this._eventTypeMap.ContainsKey(eventType.Name))
        {
            return;
        }
        this._eventTypeMap = this._eventTypeMap.Add(eventType.Name, eventType);
    }
}
