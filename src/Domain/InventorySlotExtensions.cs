using SleepingBearSystems.CraftingTools.Common;
using SleepingBearSystems.Tools.Railway;

namespace SleepingBearSystems.CraftingTools.Domain;

/// <summary>
/// Extension methods for <see cref="InventorySlot"/>.
/// </summary>
public static class InventorySlotExtensions
{
    public static Result<InventorySlot> ToInventorySlot(this InventorySlotPoco? poco, IItemRepository itemRepository, string? resultTag = default)
    {
        return poco
            .ToResultIsNotNull(failureMessage: "POCO cannot be null", resultTag)
            .OnSuccess(validPoco =>
            {
                return validPoco.Count
                    .As<int>(tag: resultTag)
                    .OnSuccess(validCount => (validPoco, validCount).ToResult(resultTag));
            })
            .OnSuccess(tuple =>
            {
                var (validPoco, validCount) = tuple;
                var validItem = validPoco.Item.ToItem(itemRepository);
                return InventorySlot.FromParameters(validItem, validCount, resultTag);
            });
    }
}
