using System.Collections.Immutable;
using CraftingTools.Shared;

namespace CraftingTools.Domain;

public sealed class ItemName : ValueObject
{
    private ItemName(string value)
    {
        this.Value = value;
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
    
    public string Value { get; }

    public static RailwayResult<ItemName> FromParameter(string? value)
    {
        var failures = ImmutableList<RailwayResultBase>.Empty;

        var validValue = value
            .ToResultIsNotNullOrWhitespace(failureMessage: "Value cannot be null, empty or whitespace.")
            .Check(v => v.Length <= 128, failureMessage: "Item name cannot exceed 128 characters in length.")
            .UnwrapOrAddToFailuresImmutable(ref failures);

        return failures.IsEmpty
            ? RailwayResult<ItemName>.Success(new ItemName(validValue))
            : RailwayResult<ItemName>.Failure(failures.ToError(message: "Unable to construct item name."));
    }
}