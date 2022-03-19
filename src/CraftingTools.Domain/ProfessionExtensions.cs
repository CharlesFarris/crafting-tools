using CraftingTools.Common;
using SleepingBearSystems.Railway;

namespace CraftingTools.Domain;

/// <summary>
/// Extension methods for the <see cref="Profession"/> class.
/// </summary>
public static class ProfessionExtensions
{
    /// <summary>
    /// Checks if a <see cref="Profession"/> instance is not null or not the <see cref="Profession.None"/> instance
    /// and wraps the instance in a <see cref="Result{TValue}"/>.
    /// </summary>
    public static Result<Profession> ToValidResult(this Profession? profession, string? resultId = default)
    {
        return profession
            .ToResultIsNotNull(failureMessage: "Profession cannot be null", resultId)
            .Check(value => value != Profession.None, failureMessage: "Profession cannot be none.");
    }
}
