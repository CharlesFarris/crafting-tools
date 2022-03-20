using System.Collections.Immutable;
using CraftingTools.Common;
using SleepingBearSystems.Railway;

namespace CraftingTools.Domain;

/// <summary>
/// Extension methods for the <see cref="Recipe"/> class.
/// </summary>
public static class RecipeExtensions
{
    /// <summary>
    /// Checks if <see cref="Recipe"/> instance is not null and not the <see cref="Recipe.None"/> instance
    /// and wraps the instance in a <see cref="Result{TValue}"/>.
    /// </summary>
    public static Result<Recipe> ToValidResult(this Recipe recipe, string? resultId = default)
    {
        return recipe
            .ToResultIsNotNull(failureMessage: "Recipe cannot be null.", resultId ?? nameof(recipe))
            .Check(value => value != Recipe.None, failureMessage: "Recipe cannot be none.");
    }

    public static Result<Recipe> SetOutput(
        this Recipe recipe,
        RecipeOutput output,
        string? resultId = default)
    {
        var failures = ImmutableList<ResultBase>.Empty;

        var validRecipe = recipe
            .ToValidResult(nameof(recipe))
            .UnwrapOrAddToFailuresImmutable(ref failures);

        var validOutput = output
            .ToValidResult(nameof(output))
            .UnwrapOrAddToFailuresImmutable(ref failures);

        return validRecipe.Output.Item == validOutput.Item && validRecipe.Output.Count == validOutput.Count
            ? Result<Recipe>.Success(validRecipe, resultId)
            : Recipe.FromParameters(validRecipe.Id, validRecipe.Profession, output, validRecipe.Inputs, resultId);
    }

    public static Result<Recipe> AddInput(
        this Recipe recipe,
        RecipeInput input,
        string? resultId = default)
    {
        var failures = ImmutableList<ResultBase>.Empty;

        var validRecipe = recipe
            .ToValidResult(nameof(recipe))
            .UnwrapOrAddToFailuresImmutable(ref failures);

        var validInput = input
            .ToValidResult(nameof(input))
            .UnwrapOrAddToFailuresImmutable(ref failures);

        return Recipe.FromParameters(
            validRecipe.Id,
            validRecipe.Profession,
            validRecipe.Output,
            validRecipe.Inputs.Add(validInput),
            resultId);
    }

    public static Result<Recipe> AddInput(
        this Recipe recipe,
        Item item,
        int count,
        string? resultId = default)
    {
        return RecipeInput
            .FromParameters(item, count, resultId)
            .OnSuccess(input => recipe.AddInput(input, resultId));
    }

    public static Result<Recipe> DeleteInput(
        this Recipe recipe,
        Item item,
        string? resultId = default)
    {
        var failures = ImmutableList<ResultBase>.Empty;

        var validRecipe = recipe
            .ToValidResult(nameof(recipe))
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
            ? Result<Recipe>.Success(validRecipe, resultId)
            : Recipe.FromParameters(
                validRecipe.Id,
                validRecipe.Profession,
                validRecipe.Output,
                updatedInputs,
                resultId);
    }

    public static Result<Recipe> SetInput(
        this Recipe recipe,
        RecipeInput input,
        string? resultId = default)
    {
        var failures = ImmutableList<ResultBase>.Empty;

        var validRecipe = recipe
            .ToValidResult(nameof(recipe))
            .UnwrapOrAddToFailuresImmutable(ref failures);

        var validInput = input
            .ToValidResult(nameof(input))
            .UnwrapOrAddToFailuresImmutable(ref failures);

        if (!failures.IsEmpty)
        {
            return Result<Recipe>.Failure(failures.ToError(message: "Unable to set input."), resultId);
        }

        var updatedInputs = validRecipe.Inputs.Remove(validInput);

        return updatedInputs.Count == validInput.Count
            ? Result<Recipe>.Success(validRecipe, resultId)
            : Recipe.FromParameters(
                validRecipe.Id,
                validRecipe.Profession,
                validRecipe.Output,
                updatedInputs.Add(validInput),
                resultId);
    }
}
