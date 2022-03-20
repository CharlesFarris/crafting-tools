using NUnit.Framework;

namespace CraftingTools.Domain.Test;

/// <summary>
/// Tests for <see cref="Inventory"/>.
/// </summary>
internal static class InventoryTests
{
    /// <summary>
    /// Validates the behavior of the <c>From</c> method.
    /// </summary>
    [Test]
    public static void From_ValidatesBehavior()
    {
        // use case: null slots
        {
            var inventory = Inventory.From(slots: default);
            Assert.That(inventory, Is.EqualTo(Inventory.Empty));
        }
    }
}
