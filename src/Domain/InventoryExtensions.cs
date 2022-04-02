using SleepingBearSystems.Tools.Railway;

namespace SleepingBearSystems.CraftingTools.Domain;

/// <summary>
/// Extension methods for <see cref="Inventory"/>.
/// </summary>
internal static class InventoryExtensions
{
    /// <summary>
    /// Combines two inventories.
    /// </summary>
    public static Result<Inventory> Add(this Inventory left, Inventory? right, string? resultTag = default)
    {
        if (left is null)
        {
            throw new ArgumentNullException(nameof(left));
        }

        return right is null || right == Inventory.Empty || right.Slots.IsEmpty
            ? left.ToResult(resultTag)
            : Inventory.FromParameters(left.Slots.Concat(right.Slots), resultTag);
    }

    /// <summary>
    /// Removes all empty inventory slots.
    /// </summary>
    public static Result<Inventory> Prune(this Inventory left, string? resultTag = default)
    {
        if (left is null)
        {
            throw new ArgumentNullException(nameof(left));
        }

        var validSlots = left.Slots.RemoveAll(slot => slot.Count == 0);
        return validSlots.Count == left.Slots.Count
            ? left.ToResult(resultTag)
            : Inventory.FromParameters(validSlots, resultTag);
    }
}
