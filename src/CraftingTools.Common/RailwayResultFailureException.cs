using System.Collections.Immutable;

namespace CraftingTools.Common;

/// <summary>
/// Custom exception used to consolidate multiple railway
/// result failures.
/// </summary>
public sealed class RailwayResultFailureException : Exception
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public RailwayResultFailureException(
        string? message,
        ImmutableList<RailwayResultBase>? failures)
        : base(message.ToSafeString())
    {
        this.Failures = failures ?? ImmutableList<RailwayResultBase>.Empty;
    }

    public ImmutableList<RailwayResultBase> Failures { get; }
}
