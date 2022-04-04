using SleepingBearSystems.CraftingTools.Common;
using SleepingBearSystems.Tools.Railway;

namespace SleepingBearSystems.CraftingTools.Domain;

/// <summary>
/// Extension methods for the <see cref="Profession"/> class.
/// </summary>
public static class ProfessionExtensions
{
    /// <summary>
    /// Checks if a <see cref="Profession"/> instance is not null or not the <see cref="Profession.None"/> instance
    /// and wraps the instance in a <see cref="Result{TValue}"/>.
    /// </summary>
    public static Result<Profession> ToResultIsNotNullOrNone(this Profession? profession, string? resultTag = default)
    {
        return profession
            .ToResultIsNotNull(failureMessage: "Profession cannot be null", resultTag)
            .Check(value => value != Profession.None, failureMessage: "Profession cannot be none.");
    }

    /// <summary>
    /// Creates a <see cref="Profession"/> instance into a
    /// <see cref="ProfessionPoco"/> instance.
    /// </summary>
    public static ProfessionPoco ToPoco(this Profession? profession)
    {
        return new ProfessionPoco
        {
            Id = profession?.Id ?? Guid.Empty,
            Name = profession?.Name.Value
        };
    }

    /// <summary>
    /// Creates a <see cref="Profession"/> instances from a
    /// <see cref="ProfessionPoco"/> instance.
    /// </summary>
    public static Result<Profession> ToItem(this ProfessionPoco? poco, string? resultId = default)
    {
        return poco
            .ToResultIsNotNull(resultId)
            .OnSuccess(validPoco => Profession.FromParameters(validPoco.Id, validPoco.Name, resultId));
    }
}
