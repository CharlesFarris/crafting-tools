using SleepingBearSystems.Tools.Common;
using SleepingBearSystems.Tools.Railway;

namespace SleepingBearSystems.CraftingTools.Domain;

public sealed class GameName : ValueObject<GameName>
{
    private GameName(string value)
    {
        this.Value = value;
    }

    public string Value { get; }

    public static readonly GameName None = new("");

    public static Result<GameName> FromParameter(object? value, string? resultTag = default)
    {
        return value
            .ToSafeString()
            .Trim()
            .ToResultIsNotNullOrEmpty(resultTag)
            .Check(validValue => validValue.Length <= 32, "Name cannot exceed 32 characters.")
            .Transform(validValue => new GameName(validValue), resultTag);
    }

    protected override bool EqualsCore(GameName other)
    {
        return this.Value == other.Value;
    }

    protected override int GetHashCodeCore()
    {
        return this.Value.GetHashCode();
    }
}
