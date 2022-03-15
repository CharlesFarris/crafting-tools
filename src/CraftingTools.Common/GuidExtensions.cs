namespace CraftingTools.Common;

public static class GuidExtensions
{
    public static RailwayResult<Guid> ToValidResult(
        this Guid uid,
        string? failureMessage = default,
        string? resultId = default)
    {
        return uid
            .ToResult(resultId)
            .Check(value => value != Guid.Empty, failureMessage ?? "Guid cannot be null.");
    }
}
