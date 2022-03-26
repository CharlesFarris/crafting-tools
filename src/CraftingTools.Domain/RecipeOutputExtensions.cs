using SleepingBearSystems.Tools.Railway;

namespace CraftingTools.Domain;

/// <summary>
/// Extension methods for the <see cref="RecipeOutput"/> class.
/// </summary>
public static class RecipeOutputExtensions
{
    /// <summary>
    /// Checks if a <see cref="RecipeOutput"/> instance is not null and not the <see cref="RecipeOutput.None"/> instance
    /// and wraps the instance in a <see cref="Result{TValue}"/>.
    /// </summary>
    public static Result<RecipeOutput> ToValidResult(this RecipeOutput? output, string? resultId = default)
    {
        return output
            .ToResultIsNotNull(failureMessage: "Output cannot be null.", resultId)
            .Check(value => value != RecipeOutput.None, failureMessage: "Output cannot be none");
    }
}
