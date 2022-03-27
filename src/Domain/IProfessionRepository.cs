using System.Collections.Immutable;
using SleepingBearSystems.Common;

namespace SleepingBearSystem.CraftingTools.Domain;

public interface IProfessionRepository
{
    Maybe<Profession> GetProfessionById(Guid id);

    ImmutableList<Profession> GetProfessions();
}
