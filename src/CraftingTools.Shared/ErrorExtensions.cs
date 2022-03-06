using System.Collections.Immutable;

namespace CraftingTools.Shared;

public static class ErrorExtensions
{
    /// <summary>
    /// Creates an <see cref="Error"/> instance from the supplied
    /// message.
    /// </summary>
    public static Error ToError(this string? message)
    {
        return new Error(message, exception: default);
    }

    /// <summary>
    /// Creates an <see cref="Error"/> instance from the supplied
    /// exception and optional message.
    /// </summary>
    public static Error ToError(this Exception? exception, string? message = default)
    {
        return new Error(message, exception);
    }

    /// <summary>
    /// Creates an <see cref="Error"/> instance from the supplied
    /// immutable list of failure results.
    /// </summary>
    public static Error ToError(
        this ImmutableList<RailwayResultBase>? failures,
        string? message = default)
    {
        return new Error(message: default, new RailwayResultFailureException(message, failures));
    }
}