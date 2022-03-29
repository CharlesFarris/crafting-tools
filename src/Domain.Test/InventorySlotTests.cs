using NUnit.Framework;
using Serilog;
using SleepingBearSystems.Tools.Common;
using SleepingBearSystems.Tools.Railway;
using SleepingBearSystems.Tools.Testing;

namespace SleepingBearSystems.CraftingTools.Domain.Test;

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
        var logger = InMemoryLogger.Create(log, timeStampFormat: string.Empty);

        // local method for writing an inventory slot to the logger
        static void LogInventorySlot(ILogger localLogger, InventorySlot localSlot)
        {
            localLogger.Information(messageTemplate: "{Item} x {Count}", localSlot.Item.Name.Value, localSlot.Count);
        }

        // use case: invalid parameters
        {
            logger.Information(messageTemplate: "use case: invalid parameters");
            var result = InventorySlot.FromParameters(item: default, count: 0, resultTag: "invalid_slot");
            result.LogResult(logger, LogInventorySlot);
        }

        using IItemRepository itemRepository = new TestItemRepository();
        var item = itemRepository.GetItemByName(name: "Water").Unwrap();

        // use case: valid parameters
        {
            logger.Information(messageTemplate: "use case: valid parameters");
            var result = InventorySlot.FromParameters(item, count: 123, resultId: "valid_slot");
            result.LogResult(logger, LogInventorySlot);
        }

        CollectionAssert.AreEqual(
            new[]
            {
                "[INF] use case: invalid parameters <s:>",
                "[INF] invalid_slot: Unable to create inventory slot. <s:>",
                "[INF]   item: Item cannot be null <s:>",
                "[INF]   count: Count must greater than 0. <s:>",

                "[INF] use case: valid parameters <s:>",
                "[INF] valid_slot: Success <s:>",
                "[INF] Water x 123 <s:>"
            },
            log,
            string.Join(Environment.NewLine, log));
    }
}
