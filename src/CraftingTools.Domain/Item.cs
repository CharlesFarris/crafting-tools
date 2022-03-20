using System.Collections.Immutable;
using CraftingTools.Common;
using SleepingBearSystems.Common;
using SleepingBearSystems.Railway;

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

    // Name of the item.
    public ItemName Name { get; }

    public static readonly Item None = new(Guid.Empty, ItemName.None);

    /// <summary>
    /// Factory method for constructing <see cref="Item"/> instances.
    /// </summary>
    public static Result<Item> FromParameters(Guid id, ItemName itemName, string? resultId = default)
    {
        var failures = ImmutableList<ResultBase>.Empty;

        var validId = id
            .ToResultNotEmpty(failureMessage: "Id cannot be empty", nameof(id))
            .UnwrapOrAddToFailuresImmutable(ref failures);

        var validItemName = itemName
            .ToResultIsNotNull(failureMessage: "Item name cannot be null.", nameof(itemName))
            .UnwrapOrAddToFailuresImmutable(ref failures);

        return failures.IsEmpty
            ? Result<Item>.Success(new Item(validId, validItemName), resultId)
            : Result<Item>.Failure(failures.ToError(message: "Unable to create item."), resultId);
    }

    /// <summary>
    /// Converts a <see cref="ItemPoco"/> instance into a <see cref="Item"/> instance.
    /// </summary>
    public static Result<Item> FromPoco(ItemPoco? poco, string? resultId = default)
    {
        return poco is null
            ? Result<Item>.Success(Item.None, resultId)
            : ItemName.FromParameter(poco.Name, resultId)
                .OnSuccess(name => { return Item.FromParameters(poco.Id, name, resultId); });
    }

}
