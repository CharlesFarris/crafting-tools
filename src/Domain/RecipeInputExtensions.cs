﻿using SleepingBearSystems.CraftingTools.Common;
using SleepingBearSystems.Tools.Railway;

namespace SleepingBearSystems.CraftingTools.Domain;

/// <summary>
/// Extension methods for the <see cref="RecipeInput"/> class.
/// </summary>
public static class RecipeInputExtensions
{
    /// <summary>
    /// Checks if <see cref="RecipeInput"/> instance is not null and not the <see cref="RecipeInput.None"/> instance
    /// and wraps the instance in a <see cref="Result{TValue}"/>.
    /// </summary>
    public static Result<RecipeInput> ToValidResult(this RecipeInput input, string? resultTag = default)
    {
        return input
            .ToResultIsNotNull(failureMessage: "Input cannot be null.", resultTag)
            .Check(value => value != RecipeInput.None, failureMessage: "Input cannot be none.");
    }

    public static bool IsValid(this RecipeInput? input)
    {
        return input is not null && input != RecipeInput.None;
    }

    /// <summary>
    /// Helper method from creating <see cref="RecipeInput"/> instance.
    /// </summary>
    public static Result<RecipeInput> ToRecipeInput(this Item item, int count, string? resultTag = default)
    {
        return RecipeInput.FromParameters(item, count, resultTag);
    }

    /// <summary>
    /// Converts a <see cref="RecipeInputPoco"/> instance into a <see cref="RecipeInput"/>
    /// instance.
    /// </summary>
    public static Result<RecipeInput> FromPoco(this RecipeInputPoco? poco, IItemRepository itemRepository,
        string? resultTag = default)
    {
        if (itemRepository is null)
        {
            throw new ArgumentNullException(nameof(itemRepository));
        }

        return poco is null
            ? RecipeInput.None
                .ToResult(resultTag)
            : itemRepository
                .GetItemById(poco.ItemId)
                .ToResult(resultTag)
                .Check(maybe => maybe.HasValue, failureMessage: "Item not found.")
                .Transform(maybe => maybe.Unwrap())
                .OnSuccess(item => RecipeInput.FromParameters(item, poco.Count), resultTag);
    }

    /// <summary>
    /// Converts a <see cref="RecipeInput"/> instance into a
    /// <see cref="RecipeInputPoco"/> instance.
    /// </summary>
    public static RecipeInputPoco ToPoco(this RecipeInput? input)
    {
        return new RecipeInputPoco
        {
            ItemId = input?.Item.Id ?? Guid.Empty,
            Count = input?.Count ?? 0
        };
    }
}
