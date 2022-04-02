using System.Collections.Immutable;
using NUnit.Framework;
using Serilog;
using SleepingBearSystems.Tools.Common;
using SleepingBearSystems.Tools.Railway;

namespace SleepingBearSystems.CraftingTools.Domain.Test;

/// <summary>
/// Tests for <see cref="Inventory"/>.
/// </summary>
internal static class InventoryTests
{
    /// <summary>
    /// Validates the behavior of the <c>FromParameters</c> method.
    /// </summary>
    [Test]
    public static void FromParameters_ValidatesBehavior()
    {
        var log = new List<string>();
        var logger = InMemoryLogger.Create(log, timeStampFormat: string.Empty);


        // local method for writing an Inventory instance to the logger
        static void LogInventory(ILogger localLogger, Inventory localInventory, string indent = "  ")
        {
            localLogger.Information(messageTemplate: "{Indent}Count: {Count}", indent, localInventory.Slots.Count);
            foreach (var slot in localInventory.Slots.OrderBy(s => s.Item.Name.Value))
            {
                localLogger.Information(messageTemplate: "{Indent}{@Name} x {Count}", indent, slot.Item.Name.Value,
                    slot.Count);
            }
        }

        // use case: null container
        {
            logger.Information(messageTemplate: "use case: null container");
            var result = Inventory.FromParameters(slots: default, resultTag: "null_container");
            result.LogResult(logger, (localLogger, localInventory) => LogInventory(localLogger, localInventory));
        }

        // create test items
        using IItemRepository itemRepository = new TestItemRepository();
        var milk = itemRepository.GetItemByName(name: "Milk").Unwrap();
        var salt = itemRepository.GetItemByName(name: "Salt").Unwrap();
        var flour = itemRepository.GetItemByName(name: "Flour").Unwrap();

        // use case: container with null/none values
        {
            logger.Information(messageTemplate: "use case: container with null values");
            var slots = ImmutableList<InventorySlot?>.Empty
                .Add(value: default)
                .Add(InventorySlot.FromParameters(milk, count: 1).Unwrap())
                .Add(InventorySlot.Empty)
                .Add(InventorySlot.FromParameters(salt, count: 2).Unwrap());
            var result = Inventory.FromParameters(slots, resultTag: "null_values");
            result.LogResult(logger, (localLogger, localInventory) => LogInventory(localLogger, localInventory));
        }

        // use case: combine duplicate items
        {
            logger.Information(messageTemplate: "use case: combine duplicate items");
            var slots = ImmutableList<InventorySlot?>.Empty
                .Add(InventorySlot.FromParameters(milk, count: 1).Unwrap())
                .Add(InventorySlot.FromParameters(milk, count: 10).Unwrap())
                .Add(InventorySlot.FromParameters(salt, count: 2).Unwrap())
                .Add(InventorySlot.FromParameters(salt, count: 20).Unwrap())
                .Add(InventorySlot.FromParameters(flour, count: 3).Unwrap());
            var result = Inventory.FromParameters(slots, resultTag: "duplicate_items");
            result.LogResult(logger, (localLogger, localInventory) => LogInventory(localLogger, localInventory));
        }

        CollectionAssert.AreEqual(
            new[]
            {
                "[INF] use case: null container <s:>",
                "[INF] null_container: Success <s:>",
                "[INF]   Count: 0 <s:>",

                "[INF] use case: container with null values <s:>",
                "[INF] null_values: Success <s:>",
                "[INF]   Count: 2 <s:>",
                "[INF]   Milk x 1 <s:>",
                "[INF]   Salt x 2 <s:>",

                "[INF] use case: combine duplicate items <s:>",
                "[INF] duplicate_items: Success <s:>",
                "[INF]   Count: 3 <s:>",
                "[INF]   Flour x 3 <s:>",
                "[INF]   Milk x 11 <s:>",
                "[INF]   Salt x 22 <s:>"
            },
            log,
            string.Join(Environment.NewLine, log));
    }
}
