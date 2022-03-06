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
    public static RailwayResult<TValue> ToResult<TValue>(this TValue value, string? id = default)
    {
        return RailwayResult<TValue>.Success(value, id);
    }

    /// <summary>
    /// Wraps the supplied string in a success result if
    /// it is not null or empty.
    /// </summary>
    public static RailwayResult<string> ToResultIsNotNullOrEmpty(
        this string? value, 
        string? failureMessage = default,
        string? id = default)
    {
        return string.IsNullOrEmpty(value)
            ? RailwayResult<string>.Failure(failureMessage.ToError(), id)
            : RailwayResult<string>.Success(value, id);
    }

    /// <summary>
    /// Wraps the supplied string instance in a success result if
    /// it is not null, empty, or whitespace.
    /// </summary>
    public static RailwayResult<string> ToResultIsNotNullOrWhitespace(
        this string? value, 
        string? failureMessage = default,
        string? id = default)
    {
        return string.IsNullOrWhiteSpace(value)
            ? RailwayResult<string>.Failure(failureMessage.ToError(), id)
            : RailwayResult<string>.Success(value, id);
    }

    /// <summary>
    /// Wraps the supplied reference type instance in a success result if
    /// it is not null.
    /// </summary>
    public static RailwayResult<TValue> ToResultIsNotNull<TValue>(
        this TValue? value,
        string? failureMessage = default,
        string? id = default)
        where TValue : class
    {
        return value is null 
            ? RailwayResult<TValue>.Failure(failureMessage.ToError(), id) 
            : RailwayResult<TValue>.Success(value, id);
    }

    /// <summary>
    /// Check the value of the supplied <see cref="RailwayResult{TValue}"/> instance
    /// using the supplied predicate.
    /// </summary>
    public static RailwayResult<TValue> Check<TValue>(
        this RailwayResult<TValue> inResult,
        Func<TValue, bool> predicate,
        string? failureMessage,
        string? id = default)
    {
        if (inResult is null)
        {
            throw new ArgumentNullException(nameof(inResult));
        }

        if (predicate is null)
        {
            throw new ArgumentNullException(nameof(predicate));
        }

        if (inResult.IsFailure)
        {
            return inResult;
        }

        return predicate.Invoke(inResult.Unwrap()) 
            ? inResult 
            : RailwayResult<TValue>.Failure(failureMessage.ToError(), id ?? inResult.Id);
    }
    
    /// <summary>
    /// Unwraps the value of the <see cref="RailwayResult{TValue}"/> instance
    /// or adds the failure to the immutable list.
    /// </summary>
    public static TValue UnwrapOrAddToFailuresImmutable<TValue>(
        this RailwayResult<TValue> result,
        ref ImmutableList<RailwayResultBase> failures)
    {
        if (result is null)
        {
            throw new ArgumentNullException(nameof(result));
        }

        if (failures is null)
        {
            throw new ArgumentNullException(nameof(failures));
        }

        if (result.Status == RailwayResultStatus.Success)
        {
            return result.Unwrap();
        }

        failures = failures.Add(result);
        return default!; // suppress nullable warning
    }
}