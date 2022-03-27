using System.Collections.Immutable;
using SleepingBearSystems.Tools.Common;
using SleepingBearSystems.Tools.Railway;

namespace SleepingBearSystems.CraftingTools.Domain;

/// <summary>
/// Domain object describing an profession in a game.
/// </summary>
public class Profession : EntityGuid
{
    /// <summary>
    /// Private constructor.
    /// </summary>
    private Profession(Guid id, ProfessionName name) : base(id)
    {
        this.Name = name;
    }

    /// <summary>
    /// Domain object instance representing an invalid <see cref="Profession"/>.
    /// </summary>
    public static readonly Profession None = new(Guid.Empty, ProfessionName.None);

    /// <summary>
    /// Name of the profession.
    /// </summary>
    public ProfessionName Name { get; }

    /// <summary>
    /// Constructs a <see cref="Profession"/> instance from the supplied parameters.
    /// </summary>
    public static Result<Profession> FromParameters(object? id, object? name, string? resultId = default)
    {
        var failures = ImmutableList<Result>.Empty;

        var validId = id
            .As<Guid>(nameof(id))
            .Check(guidValue => guidValue != Guid.Empty, failureMessage: "Id cannot be empty.", nameof(id))
            .UnwrapOrAddToFailuresImmutable(ref failures);

        var validName = ProfessionName
            .FromParameters(name, nameof(name))
            .UnwrapOrAddToFailuresImmutable(ref failures);

        return failures.IsEmpty
            ? Result<Profession>.Success(new Profession(validId, validName), resultTag)
            : Result<Profession>.Failure(failures.ToError(message: "Unable to create profession."), resultTag);
    }
}
