using SleepingBearSystems.CraftingTools.Common;
using SleepingBearSystems.Tools.Common;
using SleepingBearSystems.Tools.Railway;

namespace SleepingBearSystems.CraftingTools.Domain;

/// <summary>
/// Extension methods for <see cref="InventorySlot"/>.
/// </summary>
public static class InventorySlotExtensions
{
    public static Result<InventorySlot> ToInventorySlot(this InventorySlotPoco? poco, IItemRepository itemRepository, string? resultId = default)
    {
        return poco
            .ToResultIsNotNull(failureMessage: "POCO cannot be null", resultId)
            .OnSuccess(validPoco =>
            {
                return validPoco.Count
                    .As<int>(resultId)
                    .OnSuccess(validCount => (validPoco, validCount).ToResult(resultId));
            })
            .OnSuccess(tuple =>
            {
                var (validPoco, validCount) = tuple;
                var validItem = validPoco.Item.ToItem(itemRepository);
                return InventorySlot.FromParameters(validItem, validCount, resultId);
            });
    }
}
