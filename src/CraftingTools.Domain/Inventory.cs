using System.Collections.Immutable;
using SleepingBearSystems.Common;
using SleepingBearSystems.Railway;

namespace CraftingTools.Domain;

public sealed class Inventory
{
    private Inventory(ImmutableList<InventorySlot> slots)
    {
        this.Slots = slots;
    }

    public ImmutableList<InventorySlot> Slots { get; }

    public static readonly Inventory Empty = new(ImmutableList<InventorySlot>.Empty);

    public static Result<Inventory> From(IEnumerable<InventorySlot>? slots, string? resultId = default)
    {
        if (slots is null)
        {
            return Inventory.Empty.ToResult(resultId);
        }

        var validSlots = slots
            .GroupBy(slot => slot.Item.Id)
            .Select(group =>
                InventorySlot.FromParameters(group.First().Item, group.Sum(slot => slot.Count)).Unwrap())
            .ToImmutableList();
        return new Inventory(validSlots).ToResult(resultId);
    }
}
