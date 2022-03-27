using NUnit.Framework;

namespace SleepingBearSystems.CraftingTools.Domain.Test;

/// <summary>
/// Tests for <see cref="InventoryExtensions"/>.
/// </summary>
internal static class InventoryExtensionsTests
{
    /// <summary>
    /// Validates the <c>Add</c> method.
    /// </summary>
    [Test]
    public static void Add_ValidatesBehavior()
    {
        // use case: null this
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                var sum = default(Inventory)!.Add(Inventory.Empty);
            });
            Assert.That(ex!.ParamName, Is.EqualTo(expected: "left"));
        }
    }
}
