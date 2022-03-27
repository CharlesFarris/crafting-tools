using System.Collections.Immutable;
using SleepingBearSystems.CraftingTools.Common;
using SleepingBearSystems.Tools.Common;
using SleepingBearSystems.Tools.Railway;

namespace SleepingBearSystems.CraftingTools.Domain;

/// <summary>
/// Domain object describing an item in a game.
/// </summary>
public sealed class Item : EntityGuid
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

    /// <summary>
    /// Domain object instance representing an invalid <see cref="Item"/>.
    /// </summary>
    public static readonly Item None = new(Guid.Empty, ItemName.None);

    /// <summary>
    /// Factory method for constructing <see cref="Item"/> instances.
    /// </summary>
    public static Result<Item> FromParameters(object? id, object? name, string? resultId = default)
    {
        var failures = ImmutableList<Result>.Empty;

        var validId = id
            .As<Guid>(nameof(id))
            .Check(value => value != Guid.Empty, failureMessage: "ID cannot be empty.")
            .UnwrapOrAddToFailuresImmutable(ref failures);

        var validName = ItemName
            .FromParameters(name, nameof(name))
            .UnwrapOrAddToFailuresImmutable(ref failures);

        return failures.IsEmpty
            ? Result<Item>.Success(new Item(validId, validName), resultTag)
            : Result<Item>.Failure(failures.ToError(message: "Unable to create item."), resultTag);
    }
}
