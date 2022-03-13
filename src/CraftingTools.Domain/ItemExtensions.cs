using CraftingTools.Shared;

namespace CraftingTools.Domain;

/// <summary>
/// Extension methods for the <see cref="Item"/> class.
/// </summary>
public static class ItemExtensions
{
    /// <summary>
    /// Checks if a <see cref="Item"/> instance is not null or not the <see cref="Item.None"/> instance
    /// and wraps the instance in a <see cref="RailwayResult{TValue}"/>.
    /// </summary>
    public static RailwayResult<Item> ToValidResult(this Item? item, string? resultId = default)
    {
        return item
            .ToResultIsNotNull(failureMessage: "Item cannot be null", resultId)
            .Check(value => value != Item.None, "Item cannot be none.");
    }

    public static bool IsValid(this Item? item)
    {
        return item is not null && item != Item.None;
    }
}