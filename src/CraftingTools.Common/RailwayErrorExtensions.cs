using System.Collections.Immutable;

namespace CraftingTools.Common;

public static class RailwayErrorExtensions
{
    /// <summary>
    /// Creates an <see cref="RailwayError"/> instance from the supplied
    /// message.
    /// </summary>
    public static RailwayError ToError(this string? message)
    {
        return new RailwayError(message, exception: default);
    }

    /// <summary>
    /// Creates an <see cref="RailwayError"/> instance from the supplied
    /// exception and optional message.
    /// </summary>
    public static RailwayError ToError(this Exception? exception, string? message = default)
    {
        return new RailwayError(message, exception);
    }

    /// <summary>
    /// Creates an <see cref="RailwayError"/> instance from the supplied
    /// immutable list of failure results.
    /// </summary>
    public static RailwayError ToError(
        this ImmutableList<RailwayResultBase>? failures,
        string? message = default)
    {
        return new RailwayError(message: default, new RailwayResultFailureException(message, failures));
    }
}
