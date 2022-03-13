using System.Collections.Immutable;
using CraftingTools.Shared;

namespace CraftingTools.Domain;

/// <summary>
/// Domain object describing an in-game item.
/// </summary>
public sealed class Item : Entity
{
    /// <summary>
    /// Constructor.
    /// </summary>
    private Item(Guid id, ItemName name)
        : base(id)
    {
        this.Name = name;
    }

    /// <summary>
    /// Factory method for constructing <see cref="Item"/> instances.
    /// </summary>
    public static RailwayResult<Item> FromParameters(Guid id, ItemName itemName, string? resultId)
    {
        var failures = ImmutableList<RailwayResultBase>.Empty;

        var validId = id
            .ToValidResult("Id cannot be empty", nameof(id))
            .UnwrapOrAddToFailuresImmutable(ref failures);

        var validItemName = itemName
            .ToResultIsNotNull("Item name cannot be null.", nameof(itemName))
            .UnwrapOrAddToFailuresImmutable(ref failures);

        return failures.IsEmpty
            ? RailwayResult<Item>.Success(new Item(validId, validItemName), resultId)
            : RailwayResult<Item>.Failure(failures.ToError(message: "Unable to create item."), resultId);
    }

    // Name of the item.
    public ItemName Name { get; }

    public static readonly Item None = new(Guid.Empty, ItemName.None);
}