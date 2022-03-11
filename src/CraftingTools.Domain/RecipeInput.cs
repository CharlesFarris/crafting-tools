using System.Collections.Immutable;
using CraftingTools.Shared;

namespace CraftingTools.Domain;

public sealed class RecipeInput
{
    private RecipeInput(Recipe recipe, Guid id, Item item, int count)
    {
        this.Recipe = recipe;
        this.Id = id;
        this.Item = item;
        this.Count = count;
    }

    public Recipe Recipe { get; }

    public Guid Id { get; }

    public Item Item { get; }

    public int Count { get; }

    public static readonly RecipeInput None = new(Recipe.None, id: Guid.Empty, Item.None, count: 0);

    /// <summary>
    /// Factory method for creating a <see cref="RecipeInput"/> from the
    /// supplied parameters.
    /// </summary>
    public static RailwayResult<RecipeInput> FromParameters(Recipe recipe, Guid id, Item item, int count)
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
            .Check(value => value > 0, failureMessage: "Range must be positive.")
            .UnwrapOrAddToFailuresImmutable(ref failures);

        return failures.IsEmpty
            ? RailwayResult<RecipeInput>.Success(new RecipeInput(validRecipe, validId, validItem, validCount))
            : RailwayResult<RecipeInput>.Failure(failures.ToError("Unable to create recipe output."));
    }
}