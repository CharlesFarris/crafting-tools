using JetBrains.Annotations;

namespace CraftingTools.Shared;

/// <summary>
/// Concrete implementation of the <see cref="ResultBase"/>.
/// </summary>
/// <typeparam name="TValue">Type of the stored value.</typeparam>
public class Result<TValue> : ResultBase
{
    /// <summary>
    /// Constructor
    /// </summary>
    private Result(ResultStatus status, [CanBeNull] TValue value)
        : base(status)
    {
        this.Value = value;
    }

    /// <summary>
    /// Unwraps the stored value.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if the status is not Success.</exception>
    public TValue Unwrap() =>
        this.Status == ResultStatus.Success
            ? this.Value
            : throw new InvalidOperationException("Failure.");

    /// <summary>
    /// Create an <see cref="Result{T}"/> instance for success.
    /// </summary>
    public static Result<TValue> Success(TValue value)
    {
        return new Result<TValue>(ResultStatus.Success, value);
    }

    /// <summary>
    /// Creates a <see cref="Result{T}"/> instance for failure.
    /// </summary>
    public static Result<TValue> Failure()
    {
        return new Result<TValue>(ResultStatus.Failure, value: default);
    }
    
    /// <summary>
    /// Value stored in the result.
    /// </summary>
    [CanBeNull]
    public TValue Value { get; }
}
