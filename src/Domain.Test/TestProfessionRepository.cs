using System.Collections.Immutable;
using SleepingBearSystems.Tools.Common;

namespace SleepingBearSystems.CraftingTools.Domain.Test;

public class TestProfessionRepository : IProfessionRepository
{
    public Maybe<Profession> GetProfessionById(Guid id)
    {
        return Professions.TryGetValue(id, out var profession)
            ? profession.ToMaybe()
            : Maybe<Profession>.None;
    }

    public ImmutableList<Profession> GetProfessions()
    {
        return Professions.Values.ToImmutableList();
    }

    private static Profession CreateProfession(string id, string name) =>
        Profession.FromParameters(new Guid(id), name).Unwrap();

    private static readonly ImmutableDictionary<Guid, Profession> Professions = new[]
        {
            CreateProfession(id: "362B9515-7A5C-44B7-9708-A0FB6E48F5B5", name: "Cook"),
            CreateProfession(id: "C416A1F5-9BCF-4F6D-B6EB-A74FE88EF6AC", name: "Chemist"),
            CreateProfession(id: "685EDF25-910A-49FD-8700-E04C0C19460F", name: "Blacksmith")
        }
        .ToImmutableDictionary(p => p.Id, p => p);
}
