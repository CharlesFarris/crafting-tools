using CraftingTools.Common;
using SleepingBearSystems.Railway;

namespace CraftingTools.Domain;

/// <summary>
/// Extension methods for the <see cref="Item"/> class.
/// </summary>
public static class ItemExtensions
{
    /// <summary>
    /// Checks if a <see cref="Item"/> instance is not null or not the <see cref="Item.None"/> instance
    /// and wraps the instance in a <see cref="Result{TValue}"/>.
    /// </summary>
    public static Result<Item> ToValidResult(this Item? item, string? resultId = default)
    {
        return item
            .ToResultIsNotNull(failureMessage: "Item cannot be null", resultId)
            .Check(value => value != Item.None, failureMessage: "Item cannot be none.");
    }

    public static bool IsValid(this Item? item)
    {
        return item is not null && item != Item.None;
    }
}
