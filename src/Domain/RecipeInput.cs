using System.Collections.Immutable;
using SleepingBearSystems.Tools.Common;
using SleepingBearSystems.Tools.Railway;

namespace SleepingBearSystems.CraftingTools.Domain;

public sealed class RecipeInput : ValueObject<RecipeInput>
{
    private RecipeInput(Item item, int count)
    {
        this.Item = item;
        this.Count = count;
    }

    /// <inheritdoc cref="ValueObject{T}"/>
    protected override bool EqualsCore(RecipeInput other)
    {
        return this.Item == other.Item && this.Count == other.Count;
    }

    /// <inheritdoc cref="ValueObject{T}"/>
    protected override int GetHashCodeCore()
    {
        return (this.Item, this.Count).GetHashCode();
    }

    /// <summary>
    /// The item being used.
    /// </summary>
    public Item Item { get; }

    /// <summary>
    /// The number of items being used.
    /// </summary>
    public int Count { get; }

    public static readonly RecipeInput None = new(Item.None, count: 0);

    /// <summary>
    /// Factory method for creating a <see cref="RecipeInput"/> from the
    /// supplied parameters.
    /// </summary>
    public static Result<RecipeInput> FromParameters(Item item, int count, string? resultTag = default)
    {
        var failures = ImmutableList<Result>.Empty;

        var validItem = item
            .ToResultValid(nameof(item))
            .UnwrapOrAddToFailuresImmutable(ref failures);

        var validCount = count
            .ToResult(nameof(count))
            .Check(value => value > 0, failureMessage: "Range must be positive.")
            .UnwrapOrAddToFailuresImmutable(ref failures);

        return failures.IsEmpty
            ? Result<RecipeInput>.Success(new RecipeInput(validItem, validCount), resultTag)
            : Result<RecipeInput>.Failure(failures.ToError(message: "Unable to create recipe output."),
                resultTag);
    }
}
