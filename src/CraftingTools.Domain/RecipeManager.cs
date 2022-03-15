namespace CraftingTools.Domain;

public sealed class RecipeManager
{
    public Recipe GetRecipe(Guid id)
    {
        return this._recipes.TryGetValue(id, out var existing) ? existing : Recipe.None;
    }

    public void DeleteRecipe(Recipe recipe)
    {
        if (recipe is null)
        {
            throw new ArgumentNullException(nameof(recipe));
        }

        this._recipes.Remove(recipe.Id);
    }

    private readonly Dictionary<Guid, Recipe> _recipes = new();
}
