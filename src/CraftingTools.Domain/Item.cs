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
    private Item(Guid id)
    {
        this.Id = id;
    }

    /// <summary>
    /// Factory method for constructing <see cref="Item"/> instances.
    /// </summary>
    public static RailwayResult<Item> FromParameters(Guid id)
    {
        var failures = ImmutableList<RailwayResultBase>.Empty;

        var validId = id.ToResult()
            .Check(value => value != Guid.Empty, failureMessage: "ID cannot be empty.")
            .UnwrapOrAddToFailuresImmutable(ref failures);

        return failures.IsEmpty 
            ? RailwayResult<Item>.Success(new Item(validId)) 
            : RailwayResult<Item>.Failure(failures.ToError(message: "Unable to create item."));
    }

    /// <summary>
    /// ID of the item.
    /// </summary>
    public Guid Id { get; }
}