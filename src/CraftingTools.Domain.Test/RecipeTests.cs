using System;
using CraftingTools.Shared;
using NUnit.Framework;

namespace CraftingTools.Domain.Test;

/// <summary>
/// Tests for <see cref="Recipe"/>.
/// </summary>
internal static class RecipeTests
{
    /// <summary>
    /// Validates the behavior of the factory method
    /// </summary>
    [Test]
    public static void FromParameters_ValidatesBehavior()
    {
        // use case: invalid parameters
        {
            var result = Recipe.FromParameters(Guid.Empty, resultId: "recipe");
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo("recipe"));
            Assert.That(result.Status, Is.EqualTo(RailwayResultStatus.Failure));
            Assert.That(result.Error.Message, Is.EqualTo("Unable to create recipe."));
            Assert.That(result.Error.Exception, Is.Not.Null);
        }
        
        // use case:  valid parameters
        {
            var id = new Guid("847AA5DB-63F4-49DE-952A-3930DD38D0FB");
            var result = Recipe.FromParameters(id, resultId: "recipe");
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo("recipe"));
            var recipe = result.Unwrap();
            Assert.That(recipe.Id, Is.EqualTo(id));
        }
    }
}