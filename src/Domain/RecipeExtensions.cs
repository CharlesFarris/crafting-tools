using System.Collections.Immutable;
using SleepingBearSystems.Tools.Railway;

namespace SleepingBearSystems.CraftingTools.Domain;

/// <summary>
/// Extension methods for the <see cref="Recipe"/> class.
/// </summary>
public static class RecipeExtensions
{
    /// <summary>
    /// Checks if <see cref="Recipe"/> instance is not null and not the <see cref="Recipe.None"/> instance
    /// and wraps the instance in a <see cref="Result{TValue}"/>.
    /// </summary>
    public static Result<Recipe> ToResultIsNotNullOrNone(this Recipe recipe, string? resultTag = default)
    {
        return recipe
            .ToResultIsNotNull(failureMessage: "Recipe cannot be null.", resultTag ?? nameof(recipe))
            .Check(value => value != Recipe.None, failureMessage: "Recipe cannot be none.");
    }

    public static Result<Recipe> SetOutput(
        this Recipe recipe,
        RecipeOutput output,
        string? resultTag = default)
    {
        var failures = ImmutableList<Result>.Empty;

        var validRecipe = recipe
            .ToResultIsNotNullOrNone(nameof(recipe))
            .UnwrapOrAddToFailuresImmutable(ref failures);

        var validOutput = output
            .ToResultIsNotNullOrNone(nameof(output))
            .UnwrapOrAddToFailuresImmutable(ref failures);

        return validRecipe.Output.Item == validOutput.Item && validRecipe.Output.Count == validOutput.Count
            ? Result<Recipe>.Success(validRecipe, resultTag)
            : Recipe.FromParameters(validRecipe.Id, validRecipe.Profession, output, validRecipe.Inputs, resultTag);
    }

    public static Result<Recipe> AddInput(
        this Recipe recipe,
        RecipeInput input,
        string? resultTag = default)
    {
        var failures = ImmutableList<Result>.Empty;

        var validRecipe = recipe
            .ToResultIsNotNullOrNone(nameof(recipe))
            .UnwrapOrAddToFailuresImmutable(ref failures);

        var validInput = input
            .ToResultIsNotNullOrNone(nameof(input))
            .UnwrapOrAddToFailuresImmutable(ref failures);

        return Recipe.FromParameters(
            validRecipe.Id,
            validRecipe.Profession,
            validRecipe.Output,
            validRecipe.Inputs.Add(validInput),
            resultTag);
    }

    public static Result<Recipe> AddInput(
        this Recipe recipe,
        Item item,
        int count,
        string? resultTag = default)
    {
        return RecipeInput
            .FromParameters(item, count, resultTag)
            .OnSuccess(input => recipe.AddInput(input, resultTag));
    }

    public static Result<Recipe> DeleteInput(
        this Recipe recipe,
        Item item,
        string? resultTag = default)
    {
        var failures = ImmutableList<Result>.Empty;

        var validRecipe = recipe
            .ToResultIsNotNullOrNone(nameof(recipe))
            .UnwrapOrAddToFailuresImmutable(ref failures);

        var validItem = item
            .ToResultValid(nameof(item))
            .UnwrapOrAddToFailuresImmutable(ref failures);

        if (!failures.IsEmpty)
        {
            return Result<Recipe>.Failure(failures.ToError(message: "Unable to delete input"));
        }

        var updatedInputs = validRecipe.Inputs.RemoveAll(i => i.Item == validItem);

        return updatedInputs.Count == validRecipe.Inputs.Count
            ? Result<Recipe>.Success(validRecipe, resultTag)
            : Recipe.FromParameters(
                validRecipe.Id,
                validRecipe.Profession,
                validRecipe.Output,
                updatedInputs,
                resultTag);
    }

    public static Result<Recipe> SetInput(
        this Recipe recipe,
        RecipeInput input,
        string? resultTag = default)
    {
        var failures = ImmutableList<Result>.Empty;

        var validRecipe = recipe
            .ToResultIsNotNullOrNone(nameof(recipe))
            .UnwrapOrAddToFailuresImmutable(ref failures);

        var validInput = input
            .ToResultIsNotNullOrNone(nameof(input))
            .UnwrapOrAddToFailuresImmutable(ref failures);

        if (!failures.IsEmpty)
        {
            return Result<Recipe>.Failure(failures.ToError(message: "Unable to set input."), resultTag);
        }

        var updatedInputs = validRecipe.Inputs.Remove(validInput);

        return updatedInputs.Count == validInput.Count
            ? Result<Recipe>.Success(validRecipe, resultTag)
            : Recipe.FromParameters(
                validRecipe.Id,
                validRecipe.Profession,
                validRecipe.Output,
                updatedInputs.Add(validInput),
                resultTag);
    }
}
