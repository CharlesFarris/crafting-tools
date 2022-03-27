using System.Collections.Immutable;
using SleepingBearSystems.Common;

namespace SleepingBearSystem.CraftingTools.Domain;

public interface IItemRepository
{
    Maybe<Item> GetItemById(Guid id);

    ImmutableList<Item> GetItems();
}
