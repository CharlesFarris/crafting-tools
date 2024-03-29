﻿using System.Collections.Immutable;
using SleepingBearSystems.Tools.Railway;

namespace SleepingBearSystems.CraftingTools.Domain;

/// <summary>
/// Immutable container class describing an inventory slot.
/// </summary>
/// <remarks>
/// An inventory slot can hold 0 or more items.
/// </remarks>
public sealed class InventorySlot
{
    /// <summary>
    /// Private constructor.
    /// </summary>
    private InventorySlot(Item item, int count)
    {
        this.Item = item;
        this.Count = count;
    }

    public Item Item { get; }

    public int Count { get; }

    public static readonly InventorySlot Empty = new(Item.None, count: 0);

    /// <summary>
    /// Factory method for creating an <see cref="InventorySlot"/> instance.
    /// </summary>
    public static Result<InventorySlot> FromParameters(
        Item? item,
        int count,
        string? resultTag = default)
    {
        var failures = ImmutableList<Result>.Empty;

        var validItem = item
            .ToResultValid(nameof(item))
            .UnwrapOrAddToFailuresImmutable(ref failures);

        var validCount = count
            .ToResult(nameof(count))
            .Check(value => value > 0, failureMessage: "Count must greater than 0.")
            .UnwrapOrAddToFailuresImmutable(ref failures);

        return failures.IsEmpty
            ? Result<InventorySlot>.Success(new InventorySlot(validItem, validCount), resultTag)
            : Result<InventorySlot>.Failure(failures.ToError(message: "Unable to create inventory slot."), resultTag);
    }
}
