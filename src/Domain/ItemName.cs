using SleepingBearSystems.Tools.Common;
using SleepingBearSystems.Tools.Railway;

namespace SleepingBearSystems.CraftingTools.Domain;

/// <summary>
/// Micro-type used to represent the name of
/// a <see cref="Item"/>.
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

    public override string ToString()
    {
        return this.Value;
    }

    public string Value { get; }

    /// <summary>
    /// Domain object instance representing an invalid <see cref="ItemName"/>.
    /// </summary>
    public static readonly ItemName None = new(string.Empty);

    /// <summary>
    /// Factory method for creating <see cref="ItemName"/> instances.
    /// </summary>
    public static Result<ItemName> FromParameters(object? value, string? resultTag = default)
    {
        return value
            .As<string>(tag: resultTag)
            .Check(stringValue => !string.IsNullOrWhiteSpace(stringValue), failureMessage: "Item name cannot be empty.")
            .Check(validValue => validValue.Length <= 128, failureMessage: "Item name cannot exceed 128 characters.")
            .OnSuccess(validValue => new ItemName(validValue).ToResult(resultTag));
    }
}
