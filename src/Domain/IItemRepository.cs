using System.Collections.Immutable;
using SleepingBearSystems.Tools.Common;

namespace SleepingBearSystems.CraftingTools.Domain;

public interface IItemRepository : IDisposable
{
    Maybe<Item> GetItemById(Guid id);

    ImmutableList<Item> GetItems();

    Maybe<Item> GetItemByName(string? name, bool ignoreCase = false);
}
