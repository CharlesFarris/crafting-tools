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
    public static Result<Item> ToResultValid(this Item? item, string? resultTag = default)
    {
        return item
            .ToResultIsNotNull(failureMessage: "Item cannot be null", resultTag)
            .Check(value => value != Item.None, failureMessage: "Item cannot be none.");
    }

    /// <summary>
    /// Creates a <see cref="Item"/> instance into a <see cref="ItemPoco"/> instance.
    /// </summary>
    public static ItemPoco ToPoco(this Item? item)
    {
        return new ItemPoco
        {
            Id = item?.Id ?? Guid.Empty,
            Name = item?.Name.Value ?? string.Empty
        };
    }

    /// <summary>
    /// Creates a <see cref="Item"/> instance from a <see cref="ItemPoco"/>
    /// instance.
    /// </summary>
    public static Result<Item> ToItem(this ItemPoco? poco, string? resultId = default)
    {
        return poco
            .ToResultIsNotNull(resultId)
            .OnSuccess(validPoco => Item.FromParameters(validPoco.Id, validPoco.Name, resultId));
    }

    public static Item ToItem(this object? item, IItemRepository itemRepository, bool ignoreCase = true)
    {
        switch (item)
        {
            case null:
                return Item.None;
            case Item validItem:
                return validItem;
            default:
            {
                var id = item.As<Guid>();
                if (id.IsSuccess)
                {
                    return itemRepository
                        .GetItemById(id.Unwrap())
                        .GetValueOrDefault(Item.None)!;
                }

                var name = item.As<string>();
                return name.IsSuccess
                    ? itemRepository
                        .GetItemByName(name.Unwrap(), ignoreCase)
                        .GetValueOrDefault(Item.None)!
                    : Item.None;
            }
        }
    }
}
