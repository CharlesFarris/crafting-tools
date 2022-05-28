using System.Collections.Immutable;
using SleepingBearSystems.CraftingTools.Common;
using SleepingBearSystems.Tools.Domain;
using SleepingBearSystems.Tools.Railway;

namespace SleepingBearSystems.CraftingTools.Domain;

public class Profession : EntityGuid
{
    private Profession(Guid id, ProfessionName name) : base(id)
    {
        this.Name = name;
    }

    public static readonly Profession None = new(Guid.Empty, ProfessionName.None);
    public ProfessionName Name { get; }

    public static Result<Profession> FromParameters(Guid id, ProfessionName name, string? resultTag = default)
    {
        var failures = ImmutableList<Result>.Empty;

        var validId = id
            .ToResultNotEmpty(failureMessage: "Id cannot be empty.", nameof(id))
            .UnwrapOrAddToFailuresImmutable(ref failures);

        var validName = name
            .ToResultIsNotNull(failureMessage: "Name cannot be null", nameof(name))
            .Check(value => value != ProfessionName.None, failureMessage: "Name cannot be none.")
            .UnwrapOrAddToFailuresImmutable(ref failures);

        return failures.IsEmpty
            ? Result<Profession>.Success(new Profession(validId, validName), resultTag)
            : Result<Profession>.Failure(failures.ToError(message: "Unable to create profession."), resultTag);
    }
}
