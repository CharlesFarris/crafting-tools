namespace CraftingTools.Common;

#pragma warning disable CA1000 // Do not declare static members on generic types

/// <summary>
/// Concrete implementation of the <see cref="RailwayResultBase"/>.
/// </summary>
/// <typeparam name="TValue">Type of the stored value.</typeparam>
public class RailwayResult<TValue> : RailwayResultBase
{
    /// <summary>
    /// Constructor
    /// </summary>
    private RailwayResult(
        RailwayResultStatus status,
        RailwayError error,
        string? id,
        TValue? value)
        : base(status, error, id)
    {
        this.Value = value;
    }

    /// <summary>
    /// Unwraps the stored value.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if the status is not Success.</exception>
    public TValue Unwrap() =>
        (this.Status == RailwayResultStatus.Success
            ? this.Value
            : throw new InvalidOperationException(message: "Failure."))!; // suppress nullable warning

    /// <summary>
    /// Create an <see cref="RailwayResult{TValue}"/> instance for success.
    /// </summary>
    public static RailwayResult<TValue> Success(TValue value, string? id = default)
    {
        return new RailwayResult<TValue>(RailwayResultStatus.Success, RailwayError.Empty, id, value);
    }

    /// <summary>
    /// Creates a <see cref="RailwayResult{TValue}"/> instance for failure.
    /// </summary>
    public static RailwayResult<TValue> Failure(RailwayError error, string? id = default)
    {
        return new RailwayResult<TValue>(RailwayResultStatus.Failure, error, id, value: default);
    }

    /// <summary>
    /// Value stored in the result.
    /// </summary>
    internal TValue? Value { get; }
}
