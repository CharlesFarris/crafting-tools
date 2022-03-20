using System.Collections.Immutable;

namespace CraftingTools.Domain;

public sealed class Inventory
{
    private Inventory(ImmutableList<(Item, int)> slots)
    {
        this.Slots = slots;
    }

    public ImmutableList<(Item, int)> Slots { get; }

    public static readonly Inventory Empty = new(ImmutableList<(Item, int)>.Empty);

    public static Inventory From(IEnumerable<(Item, int)>? slots)
    {
        if (slots is null)
        {
            return Inventory.Empty;
        }

        var validSlots = slots
            .GroupBy(tuple => tuple.Item1.Id)
            .Select(group => (group.First().Item1, group.Sum(slot => Math.Max(slot.Item2, val2: 0))))
            .ToImmutableList();
        return new Inventory(validSlots);
    }
}
