using System;
using CraftingTools.Common;
using NUnit.Framework;
using SleepingBearSystems.Railway;

namespace CraftingTools.Domain.Test;

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
            var itemName = ItemName.FromParameter(value: "name").Unwrap();
            var result = Item.FromParameters(id, itemName, resultId: "resultId");
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Status, Is.EqualTo(ResultStatus.Success));
            Assert.That(result.Id, Is.EqualTo(expected: "resultId"));
            var item = result.Unwrap();
            Assert.That(item.Id, Is.EqualTo(id));
            Assert.That(item.Name.Value, Is.EqualTo(expected: "name"));
        }

        // use case: invalid ID
        {
            var itemName = ItemName.FromParameter(value: "name").Unwrap();
            var result = Item.FromParameters(Guid.Empty, itemName, resultId: "resultId");
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Status, Is.EqualTo(ResultStatus.Failure));
            Assert.That(result.Error.Message, Is.EqualTo(expected: "Unable to create item."));
            Assert.That(result.Id, Is.EqualTo(expected: "resultId"));
        }
    }
}
