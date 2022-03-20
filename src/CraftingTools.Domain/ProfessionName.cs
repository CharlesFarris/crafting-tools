using CraftingTools.Common;
using SleepingBearSystems.Common;
using SleepingBearSystems.Railway;

namespace CraftingTools.Domain;

public class ProfessionName : ValueObject<ProfessionName>
{
    private ProfessionName(string value)
    {
        this.Value = value;
    }

    protected override bool EqualsCore(ProfessionName other)
    {
        return this.Value == other.Value;
    }

    protected override int GetHashCodeCore()
    {
        return this.Value.GetHashCode();
    }

    public string Value { get; }

    public static readonly ProfessionName None = new(string.Empty);

    public static Result<ProfessionName> FromParameters(string? value, string? resultId = default)
    {
        return value
            .ToResultIsNotNullOrWhitespace(failureMessage: "Profession name cannot be empty.",
                resultId)
            .Check(validValue => validValue.Length <= 32,
                failureMessage: "Profession name cannot exceed 32 characters.")
            .OnSuccess(validValue => new ProfessionName(validValue).ToResult(resultId));
    }
}
