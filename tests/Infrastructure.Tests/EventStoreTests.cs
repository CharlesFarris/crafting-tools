using EventStore.Client;

namespace SleepingBearSystems.CraftingTools.Infrastructure.Tests;

/// <summary>
/// Tests for <see cref="EventStore"/>.
/// </summary>
internal static class Tests
{
    [Test]
    public static void Ctor_ValidatesBehavior()
    {
        var client = new EventStoreClient();
        var eventStore = new EventStore(client);
        Assert.That(eventStore, Is.Not.Null);
    }
}
