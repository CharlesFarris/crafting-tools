using System.Collections.Immutable;
using SleepingBearSystems.Tools.Common;
using SleepingBearSystems.Tools.Railway;

namespace SleepingBearSystems.CraftingTools.Domain;

/// <summary>
/// Output of a <see cref="Recipe"/>.
/// </summary>
public sealed class RecipeOutput : ValueObject<RecipeOutput>
{
    /// <summary>
    /// Constructor.
    /// </summary>
    private RecipeOutput(Item item, int count)
    {
        this.Item = item;
        this.Count = count;
    }

    /// <inheritdoc cref="ValueObject{T}"/>
    protected override bool EqualsCore(RecipeOutput other)
    {
        return this.Item == other.Item && this.Count == other.Count;
    }

    /// <inheritdoc cref="ValueObject{T}"/>
    protected override int GetHashCodeCore()
    {
        return (this.Item, this.Count).GetHashCode();
    }

    /// <summary>
    /// The item being created.
    /// </summary>
    public Item Item { get; }

    /// <summary>
    /// The number of items being created.
    /// </summary>
    public int Count { get; }

    public static readonly RecipeOutput None = new(Item.None, count: 0);

    /// <summary>
    /// Factory method for creating a <see cref="RecipeOutput"/> instance
    /// from the supplied parameters.
    /// </summary>
    public static Result<RecipeOutput> FromParameters(Item item, int count, string? resultTag = default)
    {
        var failures = ImmutableList<Result>.Empty;

        var validItem = item
            .ToResultValid(nameof(item))
            .UnwrapOrAddToFailuresImmutable(ref failures);

        var validCount = count
            .ToResult(nameof(count))
            .Check(value => value > 0, failureMessage: "Count must be positive.")
            .UnwrapOrAddToFailuresImmutable(ref failures);

        return failures.IsEmpty
            ? Result<RecipeOutput>.Success(new RecipeOutput(validItem, validCount), resultTag)
            : Result<RecipeOutput>.Failure(failures.ToError(message: "Unable to create recipe output."),
                resultTag);
    }
}
