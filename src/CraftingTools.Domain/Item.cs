using System.Collections.Immutable;
using CraftingTools.Shared;

namespace CraftingTools.Domain;

/// <summary>
/// Domain object describing an in-game item.
/// </summary>
public sealed class Item
{
    /// <summary>
    /// Constructor.
    /// </summary>
    private Item(Guid id, ItemName name)
    {
        this.Id = id;
        this.Name = name;
    }

    /// <summary>
    /// Factory method for constructing <see cref="Item"/> instances.
    /// </summary>
    public static RailwayResult<Item> FromParameters(Guid id, string? name, string? resultId)
    {
        var failures = ImmutableList<RailwayResultBase>.Empty;

        var validId = id
            .ToResult(nameof(id))
            .Check(value => value != Guid.Empty, failureMessage: "ID cannot be empty.")
            .UnwrapOrAddToFailuresImmutable(ref failures);

        var itemName = ItemName
            .FromParameter(name, nameof(name))
            .UnwrapOrAddToFailuresImmutable(ref failures);

        return failures.IsEmpty 
            ? RailwayResult<Item>.Success(new Item(validId, itemName), resultId) 
            : RailwayResult<Item>.Failure(failures.ToError(message: "Unable to create item."), resultId);
    }

    /// <summary>
    /// ID of the item.
    /// </summary>
    public Guid Id { get; }

    // Name of the item.
    public ItemName Name { get; }
}