using CraftingTools.Shared;

namespace CraftingTools.Domain;

/// <summary>
/// Extension methods for the <see cref="RecipeInput"/> class.
/// </summary>
public static class RecipeInputExtensions
{
    /// <summary>
    /// Checks if <see cref="RecipeInput"/> instance is not null and not the <see cref="RecipeInput.None"/> instance
    /// and wraps the instance in a <see cref="RailwayResult{TValue}"/>.
    /// </summary>
    public static RailwayResult<RecipeInput> ToValidResult(this RecipeInput input, string? resultId = default)
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
    public static RailwayResult<RecipeInput> ToRecipeInput(this Item item, int count, string? resultId = default)
    {
        return RecipeInput.FromParameters(item, count, resultId);
    }
}