using System.Collections.Immutable;
using System.Text;
using System.Text.Json;
using EventStore.Client;
using SleepingBearSystems.CraftingTools.Application;
using SleepingBearSystems.CraftingTools.Domain;
using SleepingBearSystems.Tools.Common;
using SleepingBearSystems.Tools.Railway;

namespace SleepingBearSystems.CraftingTools.Infrastructure;

public sealed class EventStore : IEventStore
{
    private readonly EventStoreClient _client;

    private ImmutableDictionary<string, Type> _eventTypeMap = ImmutableDictionary<string, Type>.Empty;

    private EventStore(EventStoreClient client)
    {
        this._client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async Task<Unit> AppendEvents(
        string streamName,
        ImmutableList<IEvent> events,
        CancellationToken cancellationToken)
    {
        var eventData = events.Select(@event =>
        {
            var data = JsonSerializer.SerializeToUtf8Bytes(@event, @event.GetType());
            return new EventData(
                Uuid.NewUuid(),
                @event.GetType().Name,
                data);
        });
        await this._client.AppendToStreamAsync(
            streamName,
            StreamState.Any,
            eventData,
            cancellationToken: cancellationToken);
        return Unit.Default;
    }

    public async Task<ImmutableList<IEvent>> GetEvents(string streamName, CancellationToken cancellationToken)
    {
        var readStreamResult = this._client.ReadStreamAsync(
            Direction.Forwards,
            streamName,
            StreamPosition.Start,
            cancellationToken: cancellationToken);
        var events = ImmutableList<IEvent>.Empty;
        await foreach (var resolvedEvent in readStreamResult)
        {
            if (!this._eventTypeMap.TryGetValue(resolvedEvent.Event.EventType, out var eventType))
            {
                throw new InvalidOperationException($"Unknown event type: {resolvedEvent.Event.EventType}.");
            }

            var json = Encoding.UTF8.GetString(resolvedEvent.Event.Data.Span);
            var obj = JsonSerializer.Deserialize(json, eventType);
            events = obj is IEvent @event
                ? events.Add(@event)
                : throw new InvalidOperationException($"Invalid event type: {eventType.Name}");
        }

        return events;
    }

    /// <summary>
    /// Registers an event type for deserialization.
    /// </summary>
    /// <typeparam name="TEvent">The event type.</typeparam>
    public void RegisterEvent<TEvent>() where TEvent : IEvent
    {
        var eventType = typeof(TEvent);
        if (this._eventTypeMap.ContainsKey(eventType.Name))
        {
            return;
        }

        this._eventTypeMap = this._eventTypeMap.Add(eventType.Name, eventType);
    }

    /// <summary>
    /// Factory method for creating a <see cref="EventStore"/> instance from the
    /// gPRC connection string.
    /// </summary>
    /// <param name="connectionString">The connection string.</param>
    /// <returns>A result containing the event store instance.</returns>
    public static Result<EventStore> FromConnectionString(string connectionString)
    {
        var errors = ImmutableList<Result>.Empty;

        var validConnectionString = connectionString
            .ToResultIsNotNullOrEmpty(nameof(connectionString))
            .UnwrapOrAddToFailuresImmutable(ref errors);

        return errors.IsEmpty
            ? Result<EventStore>.Success(
                new EventStore(new EventStoreClient(EventStoreClientSettings.Create(validConnectionString))))
            : Result<EventStore>.Failure(errors.ToResultError("Unable to create EventStore."));
    }
}
