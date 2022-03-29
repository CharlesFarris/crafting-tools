using System.Collections.Immutable;
using SleepingBearSystems.Tools.Common;

namespace SleepingBearSystems.CraftingTools.Domain;

public interface IProfessionRepository : IDisposable
{
    Maybe<Profession> GetProfessionById(Guid id);

    ImmutableList<Profession> GetProfessions();

    Maybe<Profession> GetProfessionByName(string? name, bool ignoreCase = false);
}
