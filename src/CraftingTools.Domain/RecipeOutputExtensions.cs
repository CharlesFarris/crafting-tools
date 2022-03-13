using CraftingTools.Shared;

namespace CraftingTools.Domain;

/// <summary>
/// Extension methods for the <see cref="RecipeOutput"/> class.
/// </summary>
public static class RecipeOutputExtensions
{
    /// <summary>
    /// Checks if a <see cref="RecipeOutput"/> instance is not null and not the <see cref="RecipeOutput.None"/> instance
    /// and wraps the instance in a <see cref="RailwayResult{TValue}"/>.
    /// </summary>
    public static RailwayResult<RecipeOutput> ToValidResult(this RecipeOutput? output, string? resultId = default)
    {
        return output
            .ToResultIsNotNull(failureMessage: "Output cannot be null.", resultId)
            .Check(value => value != RecipeOutput.None, failureMessage: "Output cannot be none");
    }

    public static RailwayResult<RecipeOutput> ToRecipeOutput(this Item item, int count, string? resultId  =default)
    {
        return RecipeOutput.FromParameters(item, count, resultId);
    }
}