using System;
using CraftingTools.Shared;
using NUnit.Framework;

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
            var result = Item.FromParameters(id);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Status, Is.EqualTo(RailwayResultStatus.Success));
            var item = result.Unwrap();
            Assert.That(item.Id, Is.EqualTo(id));
        }
        
        // use case: invalid ID
        {
            var result = Item.FromParameters(Guid.Empty);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Status, Is.EqualTo(RailwayResultStatus.Failure));
            Assert.That(result.Error.Message, Is.EqualTo(""));
        }
    }
}