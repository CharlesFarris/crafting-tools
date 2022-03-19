using CraftingTools.Common;
using SleepingBearSystems.Railway;

namespace CraftingTools.Domain;

/// <summary>
/// Extension methods for the <see cref="RecipeInput"/> class.
/// </summary>
public static class RecipeInputExtensions
{
    /// <summary>
    /// Checks if <see cref="RecipeInput"/> instance is not null and not the <see cref="RecipeInput.None"/> instance
    /// and wraps the instance in a <see cref="Result{TValue}"/>.
    /// </summary>
    public static Result<RecipeInput> ToValidResult(this RecipeInput input, string? resultId = default)
    {
        return input
            .ToResultIsNotNull(failureMessage: "Input cannot be null.", resultId)
            .Check(value => value != RecipeInput.None, failureMessage: "Input cannot be none.");
    }

    public static bool IsValid(this RecipeInput? input)
    {
        return input is not null && input != RecipeInput.None;
    }

    /// <summary>
    /// Helper method from creating <see cref="RecipeInput"/> instance.
    /// </summary>
    public static Result<RecipeInput> ToRecipeInput(this Item item, int count, string? resultId = default)
    {
        return RecipeInput.FromParameters(item, count, resultId);
    }
}
