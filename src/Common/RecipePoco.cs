namespace SleepingBearSystems.CraftingTools.Common;

public sealed class RecipePoco
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public Guid ProfessionId { get; set; }

    public RecipeOutputPoco? RecipeOutput { get; set; }

    public IEnumerable<RecipeInputPoco>? RecipeInputs { get; set; }
}
