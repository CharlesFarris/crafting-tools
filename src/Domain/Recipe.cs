using System.Collections.Immutable;
using SleepingBearSystems.Common;
using SleepingBearSystems.CraftingTools.Common;
using SleepingBearSystems.Tools.Railway;

namespace SleepingBearSystems.CraftingTools.Domain;

public sealed class Recipe : Entity
{
    /// <summary>
    /// Constructor.
    /// </summary>
    private Recipe(Guid id, Profession profession, RecipeOutput output, ImmutableList<RecipeInput> inputs)
        : base(id)
    {
        this.Profession = profession;
        this.Output = output;
        this.Inputs = inputs;
    }

    public Profession Profession { get; }
    public RecipeOutput Output { get; }

    public ImmutableList<RecipeInput> Inputs { get; }

    public static readonly Recipe None = new(Guid.Empty, Profession.None, RecipeOutput.None,
        ImmutableList<RecipeInput>.Empty);

    /// <summary>
    /// Factory method for creating a <see cref="Recipe"/> instance
    /// from the supplied parameters.
    /// </summary>
    public static Result<Recipe> FromParameters(
        Guid id,
        Profession profession,
        RecipeOutput? output,
        IEnumerable<RecipeInput?>? inputs,
        string? resultId = default)
    {
        var failures = ImmutableList<ResultBase>.Empty;

        var validId = id
            .ToResultNotEmpty(failureMessage: "Id cannot be empty.", nameof(id))
            .UnwrapOrAddToFailuresImmutable(ref failures);

        var validProfession = profession
            .ToValidResult(nameof(profession))
            .UnwrapOrAddToFailuresImmutable(ref failures);

        var validOutput = output
            .ToValidResult(nameof(output))
            .UnwrapOrAddToFailuresImmutable(ref failures);

        var validInputs = (inputs ?? Enumerable.Empty<RecipeInput?>())
            .Where(input => input.IsValid())
            .GroupBy(input => input!.Item.Id)
            .Select(group =>
                group.Count() > 1
                    ? RecipeInput.FromParameters(group.First()!.Item, group.Sum(i => i!.Count)).Unwrap()
                    : group.First())
            .ToImmutableList()
            .ToResult(nameof(inputs))
            .UnwrapOrAddToFailuresImmutable(ref failures);

        return failures.IsEmpty
            ? Result<Recipe>.Success(new Recipe(validId, validProfession, validOutput, validInputs!), resultId)
            : Result<Recipe>.Failure(failures.ToError(message: "Unable to create recipe."), resultId);
    }
}
