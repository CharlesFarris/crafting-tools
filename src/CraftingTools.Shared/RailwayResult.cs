namespace CraftingTools.Shared;

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
        Error error,
        TValue? value)
        : base(status, error)
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
    public static RailwayResult<TValue> Success(TValue value)
    {
        return new RailwayResult<TValue>(RailwayResultStatus.Success, Error.Empty, value);
    }

    /// <summary>
    /// Creates a <see cref="RailwayResult{TValue}"/> instance for failure.
    /// </summary>
    public static RailwayResult<TValue> Failure(Error error)
    {
        return new RailwayResult<TValue>(RailwayResultStatus.Failure, error, value: default);
    }

    /// <summary>
    /// Value stored in the result.
    /// </summary>
    public TValue? Value { get; }
}
