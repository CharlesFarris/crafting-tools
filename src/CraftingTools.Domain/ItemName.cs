using System.Collections.Immutable;
using CraftingTools.Shared;

namespace CraftingTools.Domain;

/// <summary>
/// Micro-type used to represent the name of a
/// <see cref="Item"/>.
/// </summary>
public sealed class ItemName : ValueObject<ItemName>
{
    /// <summary>
    /// Constructor.
    /// </summary>
    private ItemName(string value)
    {
        this.Value = value;
    }

    /// <inheritdoc cref="ValueObject{T}"/>
    protected override bool EqualsCore(ItemName other)
    {
        return this.Value == other.Value;
    }

    /// <inheritdoc cref="ValueObject{T}"/>
    protected override int GetHashCodeCore()
    {
        return this.Value.GetHashCode();
    }

    public string Value { get; }

    public static readonly ItemName None = new(value: string.Empty);

    /// <summary>
    /// Factory method for creating <see cref="ItemName"/> instances.
    /// </summary>
    public static RailwayResult<ItemName> FromParameter(string? value, string? resultId = default)
    {
        return value
            .ToResultIsNotNullOrWhitespace(failureMessage: "Item name cannot be empty.", resultId)
            .Check(validValue => validValue.Length <= 128, failureMessage: "Item name cannot exceed 128 characters.")
            .OnSuccess(validValue => new ItemName(validValue).ToResult(resultId));
    }
}