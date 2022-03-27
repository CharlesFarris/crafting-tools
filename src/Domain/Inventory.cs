using System.Collections.Immutable;
using SleepingBearSystems.Tools.Railway;

namespace CraftingTools.Domain;

/// <summary>
/// Immutable container class for handling item
/// inventories.
/// </summary>
public sealed class Inventory
{
    private Inventory(ImmutableList<InventorySlot> slots)
    {
        this.Slots = slots;
    }

    public ImmutableList<InventorySlot> Slots { get; }

    public static readonly Inventory Empty = new(ImmutableList<InventorySlot>.Empty);

    public static Result<Inventory> FromParameters(IEnumerable<InventorySlot?>? slots, string? resultId = default)
    {
        if (slots is null)
        {
            return Inventory.Empty.ToResult(resultId);
        }

        var validSlots = slots
            .Where(slot => slot is not null && slot != InventorySlot.Empty)
            .GroupBy(slot => slot!.Item.Id)
            .Select(group =>
                InventorySlot.FromParameters(group.First()!.Item, group.Sum(slot => slot!.Count)).Unwrap())
            .ToImmutableList();
        return new Inventory(validSlots).ToResult(resultId);
    }
}
