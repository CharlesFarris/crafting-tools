using SleepingBearSystems.CraftingTools.Common;
using SleepingBearSystems.Tools.Railway;

namespace SleepingBearSystems.CraftingTools.Domain;

/// <summary>
/// Extension methods for the <see cref="Item"/> class.
/// </summary>
public static class ItemExtensions
{
    /// <summary>
    /// Checks if a <see cref="Item"/> instance is not null or not the <see cref="Item.None"/> instance
    /// and wraps the instance in a <see cref="Result{TValue}"/>.
    /// </summary>
    public static Result<Item> ToResultValid(this Item? item, string? resultId = default)
    {
        return item
            .ToResultIsNotNull(failureMessage: "Item cannot be null", resultId)
            .Check(value => value != Item.None, failureMessage: "Item cannot be none.");
    }

    /// <summary>
    /// Converts a <see cref="Item"/> instance into a <see cref="ItemPoco"/> instance.
    /// </summary>
    public static ItemPoco ToPoco(this Item? item)
    {
        return new ItemPoco
        {
            Id = item?.Id ?? Guid.Empty,
            Name = item?.Name.Value ?? string.Empty
        };
    }
}
