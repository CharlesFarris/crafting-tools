using NUnit.Framework;
using Serilog;
using SleepingBearSystems.Tools.Common;
using SleepingBearSystems.Tools.Railway;

namespace SleepingBearSystems.CraftingTools.Domain.Test;

/// <summary>
/// Tests for <see cref="Item"/> class.
/// </summary>
internal static class ItemTests
{
    /// <summary>
    /// Validates the behavior of the <c>FromParameters</c> method.
    /// </summary>
    [Test]
    public static void FromParameters_ValidatesBehavior()
    {
        var log = new List<string>();
        var logger = InMemoryLogger.Create(log, timeStampFormat: string.Empty);

        static void SuccessAction(ILogger localLogger, Item localItem)
        {
            localLogger.Information(messageTemplate: "{Id} {Name}", localItem.Id.ToString(), localItem.Name.Value);
        }

        // use case: valid parameters
        {
            logger.Information(messageTemplate: "use case: valid construction");
            Item
                .FromParameters("5E226140-DF07-47A8-B290-21F5B7E581B6", name: "name",
                    resultTag: "valid_item")
                .LogResult(logger, SuccessAction);
        }

        // use case: invalid parameters
        {
            logger.Information(messageTemplate: "use case: invalid parameters");
            Item
                .FromParameters(id: default, name: default, resultTag: "invalid_item")
                .LogResult(logger, SuccessAction);
        }

        CollectionAssert.AreEqual(
            new[]
            {
                "[INF] use case: valid construction <s:>",
                "[INF] valid_item: Success <s:>",
                "[INF] 5e226140-df07-47a8-b290-21f5b7e581b6 name <s:>",

                "[INF] use case: invalid parameters <s:>",
                "[INF] invalid_item: Unable to create item. <s:>",
                "[INF]   id: ID cannot be empty. <s:>",
                "[INF]   name: Item name cannot be empty. <s:>"
            },
            log,
            string.Join(Environment.NewLine, log));
    }
}
