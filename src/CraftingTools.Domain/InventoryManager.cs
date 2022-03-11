using System.Collections.Immutable;
using CraftingTools.Shared;

namespace CraftingTools.Domain;

public sealed class InventoryManager
{
    public int GetCount(Item item)
    {
        if (item is null)
        {
            throw new ArgumentNullException(nameof(item));
        }

        return this._slots.GetValueOrDefault(item.Id);
    }

    public RailwayResult<int> Add(Item item, int count)
    {
        if (item is null)
        {
            throw new ArgumentNullException(nameof(item));
        }

        return count
            .ToResult(nameof(count))
            .Check(value => value > 0, failureMessage: "Count must be positive.")
            .OnSuccess(validCount =>
            {
                if (this._slots.TryGetValue(item.Id, out var existing))
                {
                    this._slots[item.Id] = existing + count;    
                }
                else
                {
                    this._slots[item.Id] = count;
                }
            });
    }

    public RailwayResult<int> Remove(Item item, int count)
    {
        if (item is null)
        {
            throw new ArgumentNullException(nameof(item));
        }

        return count
            .ToResult(nameof(count))
            .Check(value => value > 0, failureMessage: "Count must be positive.")
            .OnSuccess(validCount =>
            {
                if (!this._slots.TryGetValue(item.Id, out var existing) || count > existing)
                {
                    return RailwayResult<int>.Failure("Insufficient items in inventory.".ToError());
                }
                var remaining = existing - count;
                this._slots[item.Id] = existing - count;
                return RailwayResult<int>.Success(remaining);
            });
    }

    private readonly Dictionary<Guid, int> _slots = new();
}