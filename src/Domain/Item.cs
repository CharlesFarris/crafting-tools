using System.Collections.Immutable;
using SleepingBearSystems.CraftingTools.Common;
using SleepingBearSystems.Tools.Common;
using SleepingBearSystems.Tools.Railway;

namespace SleepingBearSystems.CraftingTools.Domain;

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
    public static Result<Item> FromParameters(Guid id, string? name, string? resultId = default)
    {
        var failures = ImmutableList<ResultBase>.Empty;

        var validId = id
            .ToResultNotEmpty(failureMessage: "Id cannot be empty.", nameof(id))
            .UnwrapOrAddToFailuresImmutable(ref failures);

        var validName = ItemName
            .FromParameter(name, nameof(name))
            .UnwrapOrAddToFailuresImmutable(ref failures);

        return failures.IsEmpty
            ? Result<Item>.Success(new Item(validId, validName), resultId)
            : Result<Item>.Failure(failures.ToError(message: "Unable to create item."), resultId);
    }

    /// <summary>
    /// Converts a <see cref="ItemPoco"/> instance into a <see cref="Item"/> instance.
    /// </summary>
    public static Result<Item> FromPoco(ItemPoco? poco, string? resultId = default)
    {
        return poco is null
            ? Result<Item>.Success(Item.None, resultId)
            : Item.FromParameters(poco.Id, poco.Name, resultId);
    }
}
