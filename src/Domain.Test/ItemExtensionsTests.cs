using NUnit.Framework;
using Serilog;
using SleepingBearSystems.CraftingTools.Common;
using SleepingBearSystems.Tools.Testing;

namespace SleepingBearSystems.CraftingTools.Domain.Test;

internal static class ItemExtensionsTests
{
    /// <summary>
    /// Validates the behavior of the <c>FromPoco</c> method.
    /// </summary>
    [Test]
    public static void FromPoco_ValidatesBehavior()
    {
        var log = new List<string>();
        var logger = TestLogger.Create(log, timeStampFormat: string.Empty);

        static void SuccessAction(ILogger localLogger, Item localItem)
        {
            localLogger.Information(messageTemplate: "{ID} {Name}", localItem.Id.ToString(), localItem.Name.Value);
        }

        // use case: valid poco
        {
            logger.Information(messageTemplate: "use case: valid poco");
            new ItemPoco
                {
                    Id = new Guid(g: "B13BA385-5AED-4AE7-9FA8-69F3D6FD24A1"),
                    Name = "name"
                }
                .FromPoco(resultId: "valid_poco")
                .LogResult(logger, SuccessAction);
        }

        // use case: null poco
        {
            logger.Information(messageTemplate: "use case: null poco");
            default(ItemPoco)
                .FromPoco(resultId: "null_poco")
                .LogResult(logger, SuccessAction);
        }

        // use case: invalid poco
        {
            logger.Information(messageTemplate: "use case: invalid poco");
            new ItemPoco
                {
                    Id = Guid.Empty
                }
                .FromPoco(resultId: "invalid_poco")
                .LogResult(logger, SuccessAction);
        }

        CollectionAssert.AreEqual(
            new[]
            {
                "[INF] use case: valid poco <s:>",
                "[INF] valid_poco: Success <s:>",
                "[INF] b13ba385-5aed-4ae7-9fa8-69f3d6fd24a1 name <s:>",

                "[INF] use case: null poco <s:>",
                "[INF] null_poco <s:>",
                "[INF] use case: invalid poco <s:>",
                "[INF] invalid_poco: Unable to create item. <s:>",
                "[INF]   id: ID cannot be empty. <s:>",
                "[INF]   name: Item name cannot be empty. <s:>",
            },
            log,
            string.Join(Environment.NewLine, log));
    }
}
