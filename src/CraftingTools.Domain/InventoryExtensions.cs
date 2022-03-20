using SleepingBearSystems.Railway;

namespace CraftingTools.Domain;

/// <summary>
/// Extension methods for <see cref="Inventory"/>.
/// </summary>
internal static class InventoryExtensions
{
    /// <summary>
    /// Combines two inventories.
    /// </summary>
    public static Result<Inventory> Add(this Inventory left, Inventory? right, string? resultId = default)
    {
        if (left is null)
        {
            throw new ArgumentNullException(nameof(left));
        }

        return right is null || right == Inventory.Empty || right.Slots.IsEmpty
            ? left.ToResult(resultId)
            : Inventory.From(left.Slots.Concat(right.Slots), resultId);
    }

    /// <summary>
    /// Removes all empty inventory slots.
    /// </summary>
    public static Result<Inventory> Prune(this Inventory left, string? resultId = default)
    {
        if (left is null)
        {
            throw new ArgumentNullException(nameof(left));
        }

        var validSlots = left.Slots.RemoveAll(slot => slot.Count == 0);
        return validSlots.Count == left.Slots.Count
            ? left.ToResult(resultId)
            : Inventory.From(validSlots, resultId);
    }
}
