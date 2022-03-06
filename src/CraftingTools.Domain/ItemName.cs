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

    public static RailwayResult<ItemName> FromParameter(string? value, string? id)
    {
        var failures = ImmutableList<RailwayResultBase>.Empty;

        var validValue = value
            .ToResultIsNotNullOrWhitespace(failureMessage: "Value cannot be null, empty or whitespace.", nameof(value))
            .Check(v => v.Length <= 128, failureMessage: "Item name cannot exceed 128 characters in length.")
            .UnwrapOrAddToFailuresImmutable(ref failures);

        return failures.IsEmpty
            ? RailwayResult<ItemName>.Success(new ItemName(validValue), id)
            : RailwayResult<ItemName>.Failure(failures.ToError(message: "Unable to construct item name."), id);
    }
}