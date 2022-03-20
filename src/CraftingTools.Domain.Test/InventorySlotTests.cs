using System;
using NUnit.Framework;
using SleepingBearSystems.Railway;

namespace CraftingTools.Domain.Test;

/// <summary>
/// Tests for <see cref="InventorySlot"/>.
/// </summary>
internal static class InventorySlotTests
{
    /// <summary>
    /// Validates the behavior of the <c>FromParameters</c> method.
    /// </summary>
    [Test]
    public static void FromParameters_ValidatesBehavior()
    {
        // use case: invalid parameters
        {
#pragma warning disable CS8625
            var result = InventorySlot.FromParameters(item: default, count: -1, resultId: "slot");
#pragma warning restore CS8625
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Status, Is.EqualTo(ResultStatus.Failure));
            CollectionAssert.AreEqual(
                new[]
                {
                    "slot: Unable to create inventory slot.",
                    "  item: Item cannot be null",
                    "  count: Count must greater than or equal to 0."
                },
                result.CollectFailureResults(),
                result.JoinCollectFailureResults(Environment.NewLine));
        }

        // use case: valid parameters
        {
            var item = Item
                .FromParameters(new Guid(g: "BCAA7FD8-99A6-4CA9-BD45-53288A96B32B"), name: "item")
                .Unwrap();
            var result = InventorySlot.FromParameters(item, count: 123, resultId: "slot");
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Status, Is.EqualTo(ResultStatus.Success));
            Assert.That(result.Id, Is.EqualTo(expected: "slot"));
            var slot = result.Unwrap();
            Assert.That(slot.Item, Is.EqualTo(item));
            Assert.That(slot.Count, Is.EqualTo(expected: 123));
        }
    }
}
