﻿using SleepingBearSystems.CraftingTools.Common;
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
    public static Result<Profession> ToValidResult(this Profession? profession, string? resultTag = default)
    {
        return profession
            .ToResultIsNotNull(failureMessage: "Profession cannot be null", resultTag)
            .Check(value => value != Profession.None, failureMessage: "Profession cannot be none.");
    }

    public static Result<Profession> FromPoco(this ProfessionPoco? poco, string? resultTag = default)
    {
        return poco is null
            ? Profession.None.ToResult(resultTag)
            : ProfessionName
                .FromParameters(poco.Name, resultTag)
                .OnSuccess(professionName => Profession.FromParameters(poco.Id, professionName));
    }
}
