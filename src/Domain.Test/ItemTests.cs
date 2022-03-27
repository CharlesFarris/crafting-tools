using NUnit.Framework;
using SleepingBearSystems.CraftingTools.Common;
using SleepingBearSystems.Tools.Common;
using SleepingBearSystems.Tools.Railway;
using SleepingBearSystems.Tools.Testing;

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
}
