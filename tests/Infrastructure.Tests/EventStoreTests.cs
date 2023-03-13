using SleepingBearSystems.CraftingTools.Application;
using SleepingBearSystems.CraftingTools.Domain;
using SleepingBearSystems.Tools.Railway;

namespace SleepingBearSystems.CraftingTools.Infrastructure.Tests;

/// <summary>
/// Tests for <see cref="EventStore"/>.
/// </summary>
internal static class Tests
{
    [Test]
    public static void Ctor_ValidatesBehavior()
    {
        var connectionString = Environment.GetEnvironmentVariable("SBS_TEST_SERVER_EVENTSTOREDB");
        Assert.That(connectionString, Is.Not.Null);
        var eventStore = EventStore
            .FromConnectionString(connectionString!)
            .Unwrap();
        var commandHandler = new ItemCommandHandler(eventStore);
        var result = commandHandler
            .Handle(new CreateItemCommand(new Guid("344138A3-5F65-4A20-87CB-8CA34EEAB557"), "Test"), new CancellationToken()).Result;
        Assert.That(result.Status, Is.EqualTo(ResultStatus.Success));
    }
}
