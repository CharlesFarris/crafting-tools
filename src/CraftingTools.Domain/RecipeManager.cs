using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using CraftingTools.Shared;

namespace CraftingTools.Domain;

public sealed class RecipeManager
{
    public RailwayResult<Recipe> CreateRecipe(Guid id)
    {
        if (this._recipes.TryGetValue(id, out var existing))
        {
            return RailwayResult<Recipe>.Failure("Recipe already exists.".ToError());
        }

        var result = Recipe
            .FromParameters(id)
            .OnSuccess(recipe =>
            {
                this._recipes.Add(recipe.Id, recipe);
            });
        return result;
    }

    public Recipe GetRecipe(Guid id)
    {
        return this._recipes.TryGetValue(id, out var existing) ? existing : Recipe.None;
    }

    public void DeleteRecipe(Guid id)
    {
        if (!this._recipes.ContainsKey(id))
        {
            return;
        }

        this._recipes.Remove(id);
        
        foreach (var inputId in this._recipeInputs
                     .Where(pair => pair.Value.Recipe.Id == id)
                     .Select(pair => pair.Key))
        {
            this._recipeInputs.Remove(inputId);
        }

        foreach (var outputId in this._recipeOutputs
                     .Where(pair => pair.Value.Recipe.Id == id)
                     .Select(pair => pair.Key))
        {
            this._recipeOutputs.Remove(outputId);
        }
    }

    public ImmutableList<RecipeOutput> GetRecipeOutputs(Recipe recipe)
    {
        if (recipe is null)
        {
            throw new ArgumentNullException(nameof(recipe));}
        
        return this._recipeOutputs.Aggregate(
            seed: ImmutableList<RecipeOutput>.Empty,
            func: (outputs, pair) => 
                pair.Value.Recipe == recipe 
                    ? outputs.Add(pair.Value) 
                    : outputs);
    }

    public ImmutableList<RecipeInput> GetRecipeInputs(Recipe recipe)
    {
        if (recipe is null)
        {
            throw new ArgumentNullException(nameof(recipe));
        }

        return this._recipeInputs.Aggregate(
            seed: ImmutableList<RecipeInput>.Empty,
            func: (inputs, pair) => pair.Value.Recipe == recipe
                ? inputs.Add(pair.Value)
                : inputs);

    }

    private readonly Dictionary<Guid, Recipe> _recipes = new();

    private readonly Dictionary<Guid, RecipeOutput> _recipeOutputs = new();

    private readonly Dictionary<Guid, RecipeInput> _recipeInputs = new();
}