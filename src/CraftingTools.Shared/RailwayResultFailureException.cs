using System.Collections.Immutable;

namespace CraftingTools.Shared;

public sealed class RailwayResultFailureException : Exception
{
    public RailwayResultFailureException(
        string? message, 
        ImmutableList<RailwayResultBase>? failures)
    : base(message.ToSafeString())
    {
        this.Failures = failures ?? ImmutableList<RailwayResultBase>.Empty;
    }

    public ImmutableList<RailwayResultBase> Failures { get; }
}