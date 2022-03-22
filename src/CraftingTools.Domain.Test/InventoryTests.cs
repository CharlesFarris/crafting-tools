using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using NUnit.Framework;
using Serilog;
using SleepingBearSystems.Testing;

namespace CraftingTools.Domain.Test;

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
        var logger = TestLogger.Create(log, timeStampFormat: string.Empty);

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
            var result = Inventory.FromParameters(slots: default, resultId: "null_container");
            result.LogResult(logger, (localLogger, localInventory) => { LogInventory(localLogger, localInventory); });
        }

        // create test items
        var item1 = Item.FromParameters(new Guid(g: "F19DF652-3092-4EC2-9B3F-E009B0024C59"), name: "item_1").Unwrap();
        var item2 = Item.FromParameters(new Guid(g: "3606DED7-1BE1-41DC-BC62-C026563AC26B"), name: "item_2").Unwrap();
        var item3 = Item.FromParameters(new Guid(g: "DBD5D58F-82FD-4E7F-8188-E553C77AC24E"), name: "item_3").Unwrap();

        // use case: container with null/none values
        {
            logger.Information(messageTemplate: "use case: container with null values");
            var slots = ImmutableList<InventorySlot?>.Empty
                .Add(value: default)
                .Add(InventorySlot.FromParameters(item1, count: 1).Unwrap())
                .Add(InventorySlot.Empty)
                .Add(InventorySlot.FromParameters(item2, count: 2).Unwrap());
            var result = Inventory.FromParameters(slots, resultId: "null_values");
            result.LogResult(logger, (localLogger, localInventory) => { LogInventory(localLogger, localInventory); });
        }

        // use case: combine duplicate items
        {
            logger.Information(messageTemplate: "use case: combine duplicate items");
            var slots = ImmutableList<InventorySlot?>.Empty
                .Add(InventorySlot.FromParameters(item1, count: 1).Unwrap())
                .Add(InventorySlot.FromParameters(item1, count: 10).Unwrap())
                .Add(InventorySlot.FromParameters(item2, count: 2).Unwrap())
                .Add(InventorySlot.FromParameters(item2, count: 20).Unwrap())
                .Add(InventorySlot.FromParameters(item3, count: 3).Unwrap());
            var result = Inventory.FromParameters(slots, resultId: "duplicate_items");
            result.LogResult(logger, (localLogger, localInventory) => { LogInventory(localLogger, localInventory); });
        }

        CollectionAssert.AreEqual(
            new[]
            {
                "[INF] use case: null container <s:>",
                "[INF] null_container: \"Success\" <s:>",
                "[INF]   Count: 0 <s:>",

                "[INF] use case: container with null values <s:>",
                "[INF] null_values: \"Success\" <s:>",
                "[INF]   Count: 2 <s:>",
                "[INF]   item_1 x 1 <s:>",
                "[INF]   item_2 x 2 <s:>",

                "[INF] use case: combine duplicate items <s:>",
                "[INF] duplicate_items: \"Success\" <s:>",
                "[INF]   Count: 3 <s:>",
                "[INF]   item_1 x 11 <s:>",
                "[INF]   item_2 x 22 <s:>",
                "[INF]   item_3 x 3 <s:>",
            },
            log,
            string.Join(Environment.NewLine, log));
    }
}
