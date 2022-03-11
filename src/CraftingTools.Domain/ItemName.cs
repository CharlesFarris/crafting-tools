using System.Collections.Immutable;
using CraftingTools.Shared;

namespace CraftingTools.Domain;

/// <summary>
/// Micro-type used to represent the name of a
/// <see cref="Item"/>.
/// </summary>
public sealed class ItemName : ValueObject
{
    /// <summary>
    /// Constructor.
    /// </summary>
    private ItemName(string value)
    {
        this.Value = value;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public string Value { get; }

    public static readonly ItemName None = new(string.Empty);

    /// <summary>
    /// Factory method for creating <see cref="ItemName"/> instances.
    /// </summary>
    public static RailwayResult<ItemName> FromParameter(string? value, string? resultId = default)
    {
        var failures = ImmutableList<RailwayResultBase>.Empty;

        var validValue = value
            .ToResultIsNotNullOrWhitespace(failureMessage: "Value cannot be null, empty or whitespace.", nameof(value))
            .Check(v => v.Length <= 128, failureMessage: "Item name cannot exceed 128 characters in length.")
            .UnwrapOrAddToFailuresImmutable(ref failures);

        return failures.IsEmpty
            ? RailwayResult<ItemName>.Success(new ItemName(validValue), resultId)
            : RailwayResult<ItemName>.Failure(failures.ToError(message: "Unable to construct item name."), resultId);
    }
}