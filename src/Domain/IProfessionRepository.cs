using System.Collections.Immutable;
using SleepingBearSystems.Tools.Common;

namespace SleepingBearSystems.CraftingTools.Domain;

public interface IProfessionRepository
{
    Maybe<Profession> GetProfessionById(Guid id);

    ImmutableList<Profession> GetProfessions();
}
