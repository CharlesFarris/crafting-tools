using System.Collections.Immutable;
using System.Diagnostics;

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

        return predicate.Invoke(inResult.Value!)
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

    /// <summary>
    /// Attempts to convert a value to the specified type and
    /// returns a railway result indicating a successful conversion.
    /// </summary>
    /// <remarks>
    /// Null values are converted to the specified type's default value.
    /// </remarks>
    public static RailwayResult<TOutput> As<TOutput>(this object? value, string? id = default)
    {
        var outputType = typeof(TOutput);
        if (outputType == typeof(string))
        {
            return (RailwayResultExtensions.AsString(value, id) as RailwayResult<TOutput>)!;
        }

        if (outputType == typeof(Guid))
        {
            return (RailwayResultExtensions.AsGuid(value, id) as RailwayResult<TOutput>)!;
        }

        if (outputType == typeof(decimal))
        {
            return (RailwayResultExtensions.AsDecimal(value, id) as RailwayResult<TOutput>)!;
        }

        return RailwayResult<TOutput>.Failure($"Conversion to {outputType.Name} is not implemented.".ToError());
    }

    public static RailwayResult<TOutput> OnSuccess<TInput, TOutput>(
        this RailwayResult<TInput> inResult,
        Func<TInput, RailwayResult<TOutput>> function,
        string? id = default)
    {
        if (inResult is null)
        {
            throw new ArgumentNullException(nameof(inResult));
        }

        if (function is null)
        {
            throw new ArgumentNullException(nameof(function));
        }

        return inResult.IsSuccess
            ? function(inResult.Unwrap())
            : RailwayResult<TOutput>.Failure(inResult.Error, id ?? inResult.Id);
    }

    public static RailwayResult<TInput> OnSuccess<TInput>(
        this RailwayResult<TInput> inResult,
        Action<TInput> action)
    {
        if (inResult is null)
        {
            throw new ArgumentNullException(nameof(inResult));
        }

        if (action is null)
        {
            throw new ArgumentNullException(nameof(action));
        }

        if (inResult.IsSuccess)
        {
            action(inResult.Unwrap());
        }

        return inResult;
    }

    private static RailwayResult<string> AsString(object? value, string? id)
    {
        return value switch
        {
            null => RailwayResult<string>.Success(value: string.Empty, id),
            string stringValue => RailwayResult<string>.Success(stringValue, id),
            _ => RailwayResult<string>.Success(value.ToSafeString(), id)
        };
    }

    private static RailwayResult<Guid> AsGuid(object? value, string? id)
    {
        return value switch
        {
            null => RailwayResult<Guid>.Success(Guid.Empty, id),
            Guid guidValue => RailwayResult<Guid>.Success(guidValue, id),
            string stringValue when Guid.TryParse(stringValue, out var parsed) => RailwayResult<Guid>.Success(parsed,
                id),
            string => RailwayResult<Guid>.Failure("Unable to parse Guid.".ToError(), id),
            _ => RailwayResult<Guid>.Failure("Unable to convert value to Guid.".ToError(), id)
        };
    }

    private static RailwayResult<decimal> AsDecimal(object? value, string? id)
    {
        return value switch
        {
            null => RailwayResult<decimal>.Success(0M, id),
            decimal decimalValue => RailwayResult<decimal>.Success(decimalValue, id),
            int intValue => RailwayResult<decimal>.Success(intValue, id),
            string stringValue when decimal.TryParse(stringValue, out var parsed) => RailwayResult<decimal>.Success(
                parsed, id),
            string => RailwayResult<decimal>.Failure("Unable to parse decimal.".ToError(), id),
            _ => RailwayResult<decimal>.Failure("Unable to convert to decimal.".ToError(),id)
        };
    }
}