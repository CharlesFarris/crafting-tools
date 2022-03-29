using System.Collections.Immutable;
using SleepingBearSystems.Tools.Common;

namespace SleepingBearSystems.CraftingTools.Domain.Test;

/// <summary>
/// Concrete implementation of the <see cref="IProfessionRepository"/>
/// interface used for testing.
/// </summary>
public sealed class TestProfessionRepository : IProfessionRepository
{
    /// <inheritdoc cref="IDisposable"/>.
    public void Dispose()
    {
    }

    /// <inheritdoc cref="IProfessionRepository"/>.
    public Maybe<Profession> GetProfessionById(Guid id)
    {
        return Professions.TryGetValue(id, out var profession)
            ? profession.ToMaybe()
            : Maybe<Profession>.None;
    }

    /// <inheritdoc cref="IProfessionRepository"/>.
    public ImmutableList<Profession> GetProfessions()
    {
        return Professions.Values.ToImmutableList();
    }

    /// <inheritdoc cref="IProfessionRepository"/>.
    public Maybe<Profession> GetProfessionByName(string? name, bool ignoreCase)
    {
        var comparison = ignoreCase
            ? StringComparison.OrdinalIgnoreCase
            : StringComparison.Ordinal;
        var profession = TestProfessionRepository.Professions.Values
            .FirstOrDefault(p => p.Name.Value.Equals(name, comparison));
        return profession?.ToMaybe() ?? Maybe<Profession>.None;
    }

    /// <inheritdoc cref="IProfessionRepository"/>.
    private static readonly ImmutableDictionary<Guid, Profession> Professions = new[]
        {
            Profession.FromParameters(id: "362B9515-7A5C-44B7-9708-A0FB6E48F5B5", name: "Cook").Unwrap(),
            Profession.FromParameters(id: "C416A1F5-9BCF-4F6D-B6EB-A74FE88EF6AC", name: "Chemist").Unwrap(),
            Profession.FromParameters(id: "685EDF25-910A-49FD-8700-E04C0C19460F", name: "Blacksmith").Unwrap()
        }
        .ToImmutableDictionary(p => p.Id, p => p);
}
