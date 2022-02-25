using System.Collections.Immutable;
using JetBrains.Annotations;

namespace CraftingTools.Shared;

/// <summary>
/// Extension methods for the <see cref="Result{TValue}"/> class.
/// </summary>
public static class ResultExtensions
{
    /// <summary>
    /// Wraps the supplied value in a success result.
    /// </summary>
    [NotNull]
    public static Result<TValue> ToResult<TValue>(this TValue value)
    {
        return Result<TValue>.Success(value);
    }
    
    /// <summary>
    /// Check the value of the supplied <see cref="Result{TValue}"/> instance
    /// using the supplied predicate.
    /// </summary>
    [NotNull]
    public static Result<TValue> Check<TValue>(this Result<TValue> result, Func<TValue, bool> predicate)
    {
        if (result is null)
        {
            throw new ArgumentNullException(nameof(result));
        }

        if (predicate is null)
        {
            throw new ArgumentNullException(nameof(predicate));
        }

        if (result.IsFailure)
        {
            return result;
        }

        return predicate.Invoke(result.Unwrap()) ? result : Result<TValue>.Failure();
    }


    /// <summary>
    /// Unwraps the value of the <see cref="Result{TValue}"/> instance
    /// or adds the failure result to the list.
    /// </summary>
    public static TValue UnwrapOrAddToFailures<TValue>([NotNull] this Result<TValue> result,
        [NotNull] List<ResultBase> failures)
    {
        if (result is null)
        {
            throw new ArgumentNullException(nameof(result));
        }

        if (failures is null)
        {
            throw new ArgumentNullException(nameof(failures));
        }

        if (result.Status == ResultStatus.Success) return result.Unwrap();
        failures.Add(result);
        return default;
    }

    /// <summary>
    /// Unwraps the value of the <see cref="Result{TValue}"/> instance
    /// or adds the failure to the immutable list.
    /// </summary>
    public static TValue UnwrapOrAddToFailuresImmutable<TValue>([NotNull] this Result<TValue> result,
        [NotNull] ref ImmutableList<ResultBase> failures)
    {
        if (result is null)
        {
            throw new ArgumentNullException(nameof(result));
        }

        if (failures is null)
        {
            throw new ArgumentNullException(nameof(failures));
        }

        if (result.Status == ResultStatus.Success)
        {
            return result.Unwrap();
        }

        failures = failures.Add(result);
        return default;
    }
}