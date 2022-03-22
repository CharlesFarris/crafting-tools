using System;
using System.Collections.Generic;
using NUnit.Framework;
using Serilog;
using Serilog.Core;
using SleepingBearSystems.Railway;
using SleepingBearSystems.Testing;

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
        var log = new List<string>();
        var logger = TestLogger.Create(log, timeStampFormat: string.Empty);

        // local method for writing an inventory slot to the logger
        static void LogInventorySlot(ILogger localLogger, InventorySlot localSlot)
        {
            localLogger.Information(messageTemplate: "{Item} x {Count}", localSlot.Item.Name.Value, localSlot.Count);
        }

        // use case: invalid parameters
        {
            var result = InventorySlot.FromParameters(item: default, count: 0, resultId: "invalid_slot");
            result.LogResult(logger, LogInventorySlot);
        }

        // use case: valid parameters
        {
            var item = Item
                .FromParameters(new Guid(g: "BCAA7FD8-99A6-4CA9-BD45-53288A96B32B"), name: "item")
                .Unwrap();
            var result = InventorySlot.FromParameters(item, count: 123, resultId: "valid_slot");
            result.LogResult(logger, LogInventorySlot);
        }

        CollectionAssert.AreEqual(
            new[]
            {
                "[INF] invalid_slot: \"Failure\" <s:>",
                "[INF] Unable to create inventory slot. <s:>",
                "SleepingBearSystems.Railway.ResultFailureException: Unable to create inventory slot.",

                "[INF] valid_slot: \"Success\" <s:>",
                "[INF] item x 123 <s:>",
            },
            log,
            string.Join(Environment.NewLine, log));
    }
}
