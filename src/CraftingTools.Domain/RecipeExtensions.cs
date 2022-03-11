using CraftingTools.Shared;

namespace CraftingTools.Domain;

public static class RecipeExtensions
{
    public static RailwayResult<RecipeOutput> CreateOutput(this Recipe recipe, Guid id, Item item, Range range)
    {
        return RecipeOutput.FromParameters(recipe, id, item, range);
    }

    public static RailwayResult<RecipeOutput> CreateOutput(this Recipe recipe, Guid id, Item item, int startCount,
        int endCount)
    {
        return RecipeOutput.FromParameters(recipe, id, item, new Range(startCount, endCount));
    }
    
    public static RailwayResult<RecipeOutput> CreateOutput(this Recipe recipe, Guid id, Item item, int count)
    {
        return RecipeOutput.FromParameters(recipe, id, item, new Range(count));
    }   
}