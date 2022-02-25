using System.Collections.Immutable;
using CraftingTools.Shared;
using JetBrains.Annotations;

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
    [NotNull]
    public static Result<Item> FromParameters(Guid id)
    {
        var failures = ImmutableList<ResultBase>.Empty;

        var validId = id.ToResult()
            .Check(value => value != Guid.Empty)
            .UnwrapOrAddToFailuresImmutable(ref failures);

        if (!failures.IsEmpty)
        {
            return Result<Item>.Failure();
        }

        var item = new Item(validId);
        return Result<Item>.Success(item);
    }

    /// <summary>
    /// ID of the item.
    /// </summary>
    public Guid Id { get; }
}