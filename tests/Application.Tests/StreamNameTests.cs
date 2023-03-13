using JetBrains.Annotations;
using SleepingBearSystems.CraftingTools.Domain;

namespace SleepingBearSystems.CraftingTools.Application.Tests;

/// <summary>
/// Tests for <see cref="StreamName"/>.
/// </summary>
internal static class StreamNameTests
{
    [Test]
    public static void FromEntity_ValidatesBehavior()
    {
        var id = new Guid("97F7C2D3-19BC-4C77-82BA-3196827C76A6");
        var streamName = StreamName.FromEntity<MockEntity>(id);
        Assert.That(streamName, Is.EqualTo($"{nameof(MockEntity)}-{id}"));
    }

    [UsedImplicitly]
    private sealed record MockEntity(Guid Id) : IEntity;
}
