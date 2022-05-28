using NUnit.Framework;
using SleepingBearSystems.CraftingTools.Common;
using SleepingBearSystems.Tools.Common;
using SleepingBearSystems.Tools.Railway;

namespace SleepingBearSystems.CraftingTools.Domain.Test;

/// <summary>
/// Tests for <see cref="Item"/> class.
/// </summary>
internal static class ItemTests
{
    /// <summary>
    /// Validates the behavior of the <c>FromParameters</c> factory
    /// method.
    /// </summary>
    [Test]
    public static void FromParameters_ValidatesBehavior()
    {
        // use case: valid construction
        {
            var id = new Guid(g: "5E226140-DF07-47A8-B290-21F5B7E581B6");
            var result = Item.FromParameters(id, name: "name", resultTag: "resultTag");
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Status, Is.EqualTo(ResultStatus.Success));
            Assert.That(result.Tag, Is.EqualTo(expected: "resultTag"));
            var item = result.Unwrap();
            Assert.That(item.Id, Is.EqualTo(id));
            Assert.That(item.Name.Value, Is.EqualTo(expected: "name"));
        }

        // use case: invalid ID
        {
            var result = Item.FromParameters(Guid.Empty, name: "name", resultTag: "resultTag");
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Status, Is.EqualTo(ResultStatus.Failure));
            Assert.That(result.Error.Message, Is.EqualTo(expected: "Unable to create item."));
            Assert.That(result.Tag, Is.EqualTo(expected: "resultTag"));
        }
    }

    /// <summary>
    /// Validates the behavior of the <c>FromPoco</c> method.
    /// </summary>
    [Test]
    public static void FromPoco_ValidatesBehavior()
    {
        var log = new List<string>();
        var logger = log.CreateInMemoryLogger(timeStampFormat: string.Empty);
        var indentationMap = IndentationMap.Create();

        // use case: valid poco
        {
            var id = new Guid(g: "B13BA385-5AED-4AE7-9FA8-69F3D6FD24A1");
            var poco = new ItemPoco
            {
                Id = id,
                Name = "name"
            };
            var result = Item.FromPoco(poco, resultTag: "poco");
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Status, Is.EqualTo(ResultStatus.Success));
            Assert.That(result.Tag, Is.EqualTo(expected: "poco"));
            var item = result.Unwrap();
            Assert.That(item.Id, Is.EqualTo(id));
            Assert.That(item.Name.Value, Is.EqualTo(expected: "name"));
        }

        // use case: null poco
        {
            var result = Item.FromPoco(poco: default, resultTag: "null");
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Status, Is.EqualTo(ResultStatus.Success));
            Assert.That(result.Tag, Is.EqualTo(expected: "null"));
            var item = result.Unwrap();
            Assert.That(item, Is.EqualTo(Item.None));
        }

        // use case: invalid poco
        {
            logger.Information(messageTemplate: "use case: invalid poco");
            var result = Item.FromPoco(
                new ItemPoco
                {
                    Id = Guid.Empty
                }, resultTag: "item");
            result.LogResult(logger, indentationMap, (_, _) => { });
        }

        CollectionAssert.AreEqual(
            new[]
            {
                "[INF] use case: invalid poco <s:>",
                "[INF] Failure (item) <s:>",
                "[INF] item: Unable to create item. <s:>",
                "[INF]     id: Id cannot be empty. <s:>",
                "[INF]     name: Item name cannot be empty. <s:>"
            },
            log,
            string.Join(Environment.NewLine, log));
    }
}
