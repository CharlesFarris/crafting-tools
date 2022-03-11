using System.Collections.Immutable;
using CraftingTools.Shared;

namespace CraftingTools.Domain;

/// <summary>
/// Output of a <see cref="Recipe"/>.
/// </summary>
public sealed class RecipeOutput
{
    /// <summary>
    /// Constructor.
    /// </summary>
    private RecipeOutput(Recipe recipe, Guid id, Item item, Range count)
    {
        this.Recipe = recipe;
        this.Id = id;
        this.Item = item;
        this.Count = count;
    }

    /// <summary>
    /// The parent recipe.
    /// </summary>
    public Recipe Recipe { get; }

    /// <summary>
    /// The ID.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// The item being created.
    /// </summary>
    public Item Item { get; }

    /// <summary>
    /// The number of items being created.
    /// </summary>
    public Range Count { get; }

    public static readonly RecipeOutput None = new(Recipe.None, id: Guid.Empty, Item.None, new Range(0));

    /// <summary>
    /// Factory method for creating a <see cref="RecipeOutput"/> instance
    /// from the supplied parameters.
    /// </summary>
    public static RailwayResult<RecipeOutput> FromParameters(Recipe recipe, Guid id, Item item, Range count)
    {
        var failures = ImmutableList<RailwayResultBase>.Empty;

        var validRecipe = recipe
            .ToResultIsNotNull(failureMessage: "Recipe cannot be null.", nameof(recipe))
            .UnwrapOrAddToFailuresImmutable(ref failures);

        var validId = id.ToResult(nameof(id))
            .Check(value => value != Guid.Empty, failureMessage: "Id cannot be empty.")
            .UnwrapOrAddToFailuresImmutable(ref failures);

        var validItem = item
            .ToResultIsNotNull(failureMessage: "Item cannot be null.", nameof(item))
            .UnwrapOrAddToFailuresImmutable(ref failures);

        var validCount = count
            .ToResult(nameof(count))
            .Check(value => value.Start > 0, failureMessage: "Range must be positive.")
            .UnwrapOrAddToFailuresImmutable(ref failures);

        return failures.IsEmpty
            ? RailwayResult<RecipeOutput>.Success(new RecipeOutput(validRecipe, validId, validItem, validCount))
            : RailwayResult<RecipeOutput>.Failure(failures.ToError("Unable to create recipe output."));
    }
}