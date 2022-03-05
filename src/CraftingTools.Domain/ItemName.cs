using System.Collections.Immutable;
using CraftingTools.Shared;
using JetBrains.Annotations;

namespace CraftingTools.Domain;

public sealed class ItemName : ValueObject
{
    private ItemName([NotNull] string value)
    {
        this.Value = value;
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
    
    [NotNull] public string Value { get; }

    public static Result<ItemName> FromParameter([CanBeNull] string value)
    {
        var errors = ImmutableList<ResultBase>.Empty;

        var validValue = value
            .ToResult()
            .Check(v => !string.IsNullOrWhiteSpace(v), "Value cannot be null, empty or whitespace.")
            .UnwrapOrAddToFailuresImmutable(ref errors);

        return errors.IsEmpty
            ? Result<ItemName>.Success(new ItemName(validValue))
            : Result<ItemName>.Failure();
    }
}