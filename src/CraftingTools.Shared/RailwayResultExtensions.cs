using System.Collections.Immutable;

namespace CraftingTools.Shared;

/// <summary>
/// Extension methods for the <see cref="RailwayResult{TValue}"/> class.
/// </summary>
public static class RailwayResultExtensions
{
    /// <summary>
    /// Wraps the supplied value in a success result.
    /// </summary>
    public static RailwayResult<TValue> ToResult<TValue>(this TValue value)
    {
        return RailwayResult<TValue>.Success(value);
    }

    /// <summary>
    /// Wraps the supplied string in a success result if
    /// it is not null or empty.
    /// </summary>
    public static RailwayResult<string> ToResultIsNotNullOrEmpty(
        this string? value, 
        string? failureMessage = default)
    {
        return string.IsNullOrEmpty(value)
            ? RailwayResult<string>.Failure(failureMessage.ToError())
            : RailwayResult<string>.Success(value);
    }

    /// <summary>
    /// Wraps the supplied string instance in a success result if
    /// it is not null, empty, or whitespace.
    /// </summary>
    public static RailwayResult<string> ToResultIsNotNullOrWhitespace(
        this string? value, 
        string? failureMessage = default)
    {
        return string.IsNullOrWhiteSpace(value)
            ? RailwayResult<string>.Failure(failureMessage.ToError())
            : RailwayResult<string>.Success(value);
    }

    /// <summary>
    /// Wraps the supplied reference type instance in a success result if
    /// it is not null.
    /// </summary>
    public static RailwayResult<TValue> ToResultNotNull<TValue>(
        this TValue? value,
        string? failureMessage = default)
        where TValue : class
    {
        return value is null 
            ? RailwayResult<TValue>.Failure(failureMessage.ToError()) 
            : RailwayResult<TValue>.Success(value);
    }

    /// <summary>
    /// Check the value of the supplied <see cref="RailwayResult{TValue}"/> instance
    /// using the supplied predicate.
    /// </summary>
    public static RailwayResult<TValue> Check<TValue>(
        this RailwayResult<TValue> railwayResult,
        Func<TValue, bool> predicate,
        string? failureMessage)
    {
        if (railwayResult is null)
        {
            throw new ArgumentNullException(nameof(railwayResult));
        }

        if (predicate is null)
        {
            throw new ArgumentNullException(nameof(predicate));
        }

        if (railwayResult.IsFailure)
        {
            return railwayResult;
        }

        return predicate.Invoke(railwayResult.Unwrap()) 
            ? railwayResult 
            : RailwayResult<TValue>.Failure(failureMessage.ToError());
    }

    /// <summary>
    /// Unwraps the value of the <see cref="RailwayResult{TValue}"/> instance
    /// or adds the failure result to the list.
    /// </summary>
    public static TValue? UnwrapOrAddToFailures<TValue>(
        this RailwayResult<TValue> railwayResult,
        List<RailwayResultBase> failures)
    {
        if (railwayResult is null)
        {
            throw new ArgumentNullException(nameof(railwayResult));
        }

        if (failures is null)
        {
            throw new ArgumentNullException(nameof(failures));
        }

        if (railwayResult.Status == RailwayResultStatus.Success)
        {
            return railwayResult.Unwrap();
        }

        failures.Add(railwayResult);
        return default;
    }

    /// <summary>
    /// Unwraps the value of the <see cref="RailwayResult{TValue}"/> instance
    /// or adds the failure to the immutable list.
    /// </summary>
    public static TValue UnwrapOrAddToFailuresImmutable<TValue>(this RailwayResult<TValue> railwayResult,
        ref ImmutableList<RailwayResultBase> failures)
    {
        if (railwayResult is null)
        {
            throw new ArgumentNullException(nameof(railwayResult));
        }

        if (failures is null)
        {
            throw new ArgumentNullException(nameof(failures));
        }

        if (railwayResult.Status == RailwayResultStatus.Success)
        {
            return railwayResult.Unwrap();
        }

        failures = failures.Add(railwayResult);
        return default!; // suppress nullable warning
    }
}