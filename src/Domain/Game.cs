using System.Collections.Immutable;
using System.Globalization;
using SleepingBearSystems.Tools.Common;
using SleepingBearSystems.Tools.Railway;

namespace SleepingBearSystems.CraftingTools.Domain;

public sealed class Game : EntityLong
{
    private Game(long id, GameName name) : base(id)
    {
        this.Name = name;
    }

    public GameName Name { get; }

    public static readonly Game None = new(0, GameName.None);

    public static Result<Game> FromParameters(object? id, object? name, string? resultTag = default)
    {
        var failures = ImmutableList<Result>.Empty;

        var validId = id
            .As<long>(tag: nameof(id))
            .Check(value => value > 0, "Id must be greater than 0.")
            .UnwrapOrAddToFailuresImmutable(ref failures);

        var validName = GameName.FromParameter(name, nameof(name))
            .UnwrapOrAddToFailuresImmutable(ref failures);

        return failures.IsEmpty
            ? Result<Game>.Success(new Game(validId, validName), resultTag)
            : Result<Game>.Failure(failures.ToError("Unable to create game."), resultTag);
    }
}
