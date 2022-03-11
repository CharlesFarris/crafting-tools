using System.Collections.Immutable;
using CraftingTools.Shared;

namespace CraftingTools.Domain;

public sealed class Recipe
{
    /// <summary>
    /// Constructor.
    /// </summary>
    private Recipe(Guid id)
    {
        this.Id = id;
    }

    /// <summary>
    /// The ID.
    /// </summary>
    public Guid Id { get; }

    public static readonly Recipe None = new(id: Guid.Empty);

    /// <summary>
    /// Factory method for creating a <see cref="Recipe"/> instance
    /// from the supplied parameters.
    /// </summary>
    public static RailwayResult<Recipe> FromParameters(Guid id, string? resultId = default)
    {
        var failures = ImmutableList<RailwayResultBase>.Empty;

        var validId = id
            .ToResult(nameof(id))
            .Check(i => i != Guid.Empty, "Guid cannot be empty.")
            .UnwrapOrAddToFailuresImmutable(ref failures);

        return failures.IsEmpty
            ? RailwayResult<Recipe>.Success(new Recipe(validId), resultId)
            : RailwayResult<Recipe>.Failure(failures.ToError("Unable to create recipe."), resultId);
    }
}